﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace ManicDigger
{
    public class MeshBatcher
    {
        public MeshBatcher()
        {
        }
        private void genlists()
        {
            int lists = GL.GenLists(listincrease);
            if (lists == 0)
            {
                throw new Exception();
            }
            this.lists.Add(lists);
        }
        public int listincrease = 1000;
        //public int nlists = 50000;
        private int GetList(int i)
        {
            while (i >= lists.Count * listincrease)
            {
                genlists();
            }
            return lists[i / listincrease] + i % listincrease;
        }
        private void ClearLists()
        {
            if (lists != null)
            {
                foreach (int l in lists)
                {
                    GL.DeleteLists(l, listincrease);
                }
            }
        }
        //int lists = -1;
        List<int> lists = new List<int>();
        int count = 0;
        public void Remove(int p)
        {
            empty[p] = 0;
        }
        struct ToAdd
        {
            public ushort[] indices;
            public VertexPositionTexture[] vertices;
            public int id;
            public bool transparent;
            public int texture;
        }
        Queue<ToAdd> toadd = new Queue<ToAdd>();
        public int Add(ushort[] p, VertexPositionTexture[] vertexPositionTexture, bool transparent, int texture)
        {
            int id;
            lock (toadd)
            {
                if (empty.Count > 0)
                {
                    id = MyLinq.First(empty.Keys);
                    empty.Remove(id);
                }
                else
                {
                    id = count;
                    count++;
                }
                toadd.Enqueue(new ToAdd() { indices = p, vertices = vertexPositionTexture, id = id,
                    transparent = transparent, texture = texture });
            }
            return id;
        }
        Dictionary<int, int> empty = new Dictionary<int, int>();
        float addperframe = 0.5f;
        float addcounter = 0;
        Vector3 playerpos;
        public void Draw(Vector3 playerpos)
        {
            this.playerpos = playerpos;
            lock (toadd)
            {
                addcounter += addperframe;
                while (//addcounter >= 1 &&
                    toadd.Count > 0)
                {
                    addcounter -= 1;
                    ToAdd t = toadd.Dequeue();
                    GL.NewList(GetList(t.id), ListMode.Compile);

                    GL.BindTexture(TextureTarget.Texture2D, t.texture);
                    GL.EnableClientState(EnableCap.TextureCoordArray);
                    GL.EnableClientState(EnableCap.VertexArray);
                    GL.EnableClientState(EnableCap.ColorArray);
                    unsafe
                    {
                        fixed (VertexPositionTexture* p = t.vertices)
                        {
                            GL.VertexPointer(3, VertexPointerType.Float, StrideOfVertices, (IntPtr)(0 + (byte*)p));
                            GL.TexCoordPointer(2, TexCoordPointerType.Float, StrideOfVertices, (IntPtr)(12 + (byte*)p));
                            GL.ColorPointer(4, ColorPointerType.UnsignedByte, StrideOfVertices, (IntPtr)(20 + (byte*)p));
                            GL.DrawElements(BeginMode.Triangles, t.indices.Length, DrawElementsType.UnsignedShort, t.indices);
                        }
                    }
                    GL.DisableClientState(EnableCap.TextureCoordArray);
                    GL.DisableClientState(EnableCap.VertexArray);
                    GL.DisableClientState(EnableCap.ColorArray);

                    /*
                    GL.Begin(BeginMode.Triangles);
                    for (int ii = 0; ii < t.indices.Length; ii++)
                    {
                        var v = t.vertices[t.indices[ii]];
                        GL.TexCoord2(v.u, v.v);
                        GL.Vertex3(v.Position.X, v.Position.Y, v.Position.Z);
                    }
                    GL.End();
                    */
                    GL.EndList();
                    GetListInfo(t.id).indicescount = t.indices.Length;
                    GetListInfo(t.id).center = t.vertices[0].Position;//todo
                    GetListInfo(t.id).transparent = t.transparent;
                }
                if (toadd.Count == 0)
                {
                    addcounter = 0;
                }
            }
            if (tocall == null)
            {
                tocall = new int[MAX_DISPLAY_LISTS];
            }
            int tocallpos = 0;
            for (int i = 0; i < count; i++)
            {
                if (!empty.ContainsKey(i))
                {
                    if (!GetListInfo(i).transparent)
                    {
                        tocall[tocallpos] = GetList(i);
                        tocallpos++;
                    }
                }
            }
            GL.CallLists(tocallpos, ListNameType.Int, tocall);
            tocallpos = 0;
            GL.Disable(EnableCap.CullFace);//for water.
            for (int i = 0; i < count; i++)
            {
                if (!empty.ContainsKey(i))
                {
                    if (GetListInfo(i).transparent)
                    {
                        tocall[tocallpos] = GetList(i);
                        tocallpos++;
                    }
                }
            }
            GL.CallLists(tocallpos, ListNameType.Int, tocall);
            GL.Enable(EnableCap.CullFace);
            //depth sorting. is it needed?
            /*
            List<int> alldrawlists = new List<int>();
            for (int i = 0; i < count; i++)
            {
                alldrawlists.Add(i);
            }
            alldrawlists.Sort(f);
            GL.CallLists(count, ListNameType.Int, alldrawlists.ToArray());
            */
        }
        public int MAX_DISPLAY_LISTS = 32 * 1024;
        int[] tocall;
        int strideofvertices = -1;
        int StrideOfVertices
        {
            get
            {
                if (strideofvertices == -1) strideofvertices = BlittableValueType.StrideOf(new VertexPositionTexture());
                return strideofvertices;
            }
        }
        class ListInfo
        {
            public int indicescount;
            public Vector3 center;
            public bool transparent;
        }
        /// <summary>
        /// Indices count in list.
        /// </summary>
        List<ListInfo> listinfo = new List<ListInfo>();
        private ListInfo GetListInfo(int id)
        {
            while (id >= listinfo.Count)
            {
                listinfo.Add(new ListInfo());
            }
            return listinfo[id];
        }
        public void Clear()
        {
            ClearLists();
            count = 0;
            empty.Clear();
            toadd.Clear();
            listinfo = new List<ListInfo>();
        }
        public int TotalTriangleCount
        {
            get
            {
                if (listinfo == null)
                {
                    return 0;
                }
                int sum = 0;
                for (int i = 0; i < count; i++)
                {
                    if (!empty.ContainsKey(i))
                    {
                        if (i < listinfo.Count)
                        {
                            sum += GetListInfo(i).indicescount;
                        }
                    }
                }
                return sum / 3;
            }
        }
    }
}

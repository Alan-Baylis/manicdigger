﻿using System;
using System.Collections.Generic;
using System.Text;
using ManicDigger;
using System.IO;
using System.Xml;
using System.Windows.Forms;
using System.Diagnostics;
using ManicDigger.Network;
using ManicDigger.Renderers;
using System.Threading;
using Utilities;

namespace GameModeFortress
{
    public class GMFortress : IInternetGameFactory, ICurrentShadows, ISinglePlayer, IOnlineGame
    {
        public IGameExit exit;
        ManicDiggerGameWindow w;
        AudioOpenAl audio;

        public ServerConnectInfo connectinfo
        {
            get;
            set;
        }

        public void Start()
        {
            w = new ManicDiggerGameWindow();
            audio = new AudioOpenAl();
            w.audio = audio;
            MakeGame(true);

            // Defaults for connectioninfo
            if (connectinfo == null)
            {
                connectinfo = new ServerConnectInfo();
                connectinfo.url = "127.0.0.1";
                connectinfo.port = 25570;
                connectinfo.username = "Local";
            }
            
            // Temporary plugin of ServerConnectInfo.  Need to come back and clean this up.
            if (connectinfo.url != null)
            {
                w.GameUrl = connectinfo.url + ":" + connectinfo.port;
            }
            if (connectinfo.username != null)
            {
                w.username = connectinfo.username;
            }

            this.exit = w;
            w.Run();
        }
        private void MakeGame(bool singleplayer)
        {
            var gamedata = new GameDataTilesManicDigger();
            var clientgame = new GMFortressLogic();
            ICurrentSeason currentseason = clientgame;
            gamedata.CurrentSeason = currentseason;
            INetworkClientFortress network;
            if (singleplayer)
            {
                network = new NetworkClientDummyInfinite() { gameworld = clientgame };
                clientgame.Players[0] = new Player() { Name = "gamer1" };
            }
            else
            {
                network = new NetworkClientFortress();
            }            
            var mapstorage = clientgame;
            var getfile = new GetFilePath(new[] { "mine", "minecraft" });
            var config3d = new Config3d();
            var mapManipulator = new MapManipulator();
            var terrainDrawer = new TerrainRenderer();
            var the3d = w;
            var exit = w;
            var localplayerposition = w;
            var worldfeatures = new WorldFeaturesDrawerDummy();
            var physics = new CharacterPhysics();
            var mapgenerator = new MapGeneratorPlain();
            var internetgamefactory = this;
            if (singleplayer)
            {
                var n = (NetworkClientDummyInfinite)network;
                n.players = clientgame;
                n.localplayerposition = localplayerposition;
            }
            else
            {
                var n = (NetworkClientFortress)network;
                n.Map = w;
                n.Clients = clientgame;
                n.Chatlines = w;
                n.Position = localplayerposition;
                n.ENABLE_FORTRESS = true;
                n.NetworkPacketReceived = clientgame;
            }
            terrainDrawer.the3d = the3d;
            terrainDrawer.getfile = getfile;
            terrainDrawer.config3d = config3d;
            terrainDrawer.mapstorage = clientgame;
            terrainDrawer.data = gamedata;
            terrainDrawer.exit = exit;
            terrainDrawer.localplayerposition = localplayerposition;
            terrainDrawer.worldfeatures = worldfeatures;
            terrainDrawer.OnCrash += (a, b) => { CrashReporter.Crash(b.exception); };
            var blockdrawertorch = new BlockDrawerTorch();
            blockdrawertorch.terraindrawer = terrainDrawer;
            blockdrawertorch.data = gamedata;
            var terrainChunkDrawer = new TerrainChunkRenderer();
            terrainChunkDrawer.config3d = config3d;
            terrainChunkDrawer.data = gamedata;
            terrainChunkDrawer.mapstorage = clientgame;
            terrainDrawer.terrainchunkdrawer = terrainChunkDrawer;
            terrainChunkDrawer.blockdrawertorch = blockdrawertorch;
            terrainChunkDrawer.terrainrenderer = terrainDrawer;
            mapManipulator.getfile = getfile;
            mapManipulator.mapgenerator = mapgenerator;
            w.map = clientgame.mapforphysics;
            w.physics = physics;
            w.clients = clientgame;
            w.network = network;
            w.data = gamedata;
            w.getfile = getfile;
            w.config3d = config3d;
            w.mapManipulator = mapManipulator;
            w.terrain = terrainDrawer;
            w.PickDistance = 4.5f;
            weapon = new WeaponBlockInfo() { data = gamedata, terrain = terrainDrawer, viewport = w, map = clientgame, shadows = shadowssimple };
            w.weapon = new WeaponRenderer() { info = weapon, blockdrawertorch = blockdrawertorch, playerpos = w };
            var playerdrawer = new CharacterRendererMonsterCode();
            playerdrawer.Load(new List<string>(File.ReadAllLines(getfile.GetFile("player.mdc"))));
            w.characterdrawer = playerdrawer;
            w.particleEffectBlockBreak = new ParticleEffectBlockBreak() { data = gamedata, map = clientgame, terrain = terrainDrawer };
            w.ENABLE_FINITEINVENTORY = false;
            clientgame.terrain = terrainDrawer;
            clientgame.viewport = w;
            clientgame.data = gamedata;
            clientgame.network = network;
            clientgame.craftingtabletool = new CraftingTableTool() { map = mapstorage };
            InfiniteMapChunked map = new InfiniteMapChunked() { generator = new WorldGeneratorDummy() };
            map.Reset(10 * 1000, 10 * 1000, 128);
            clientgame.map = map;
            terrainDrawer.ischunkready = map;
            w.game = clientgame;
            w.login = new LoginClientDummy();
            w.internetgamefactory = internetgamefactory;
            PlayerSkinDownloader playerskindownloader = new PlayerSkinDownloader();
            playerskindownloader.exit = w;
            playerskindownloader.the3d = the3d;
            playerskindownloader.skinserver = "http://fragmer.net/md/skins/";
            w.playerskindownloader = playerskindownloader;
            w.fpshistorygraphrenderer = new FpsHistoryGraphRenderer() { draw = w, viewportsize = w };
            physics.map = clientgame.mapforphysics;
            physics.data = gamedata;
            mapgenerator.data = gamedata;
            audio.getfile = getfile;
            audio.gameexit = w;
            this.clientgame = clientgame;
            this.map = map;
            w.currentshadows = this;
            shadowsfull = new Shadows() { data = gamedata, map = clientgame, terrain = terrainDrawer,
                localplayerposition = localplayerposition, config3d = config3d, ischunkready = map };
            shadowssimple = new ShadowsSimple() { data = gamedata, map = clientgame };
            if (fullshadows)
            {
                UseShadowsFull();
            }
            else
            {
                UseShadowsSimple();
            }
            if (Debugger.IsAttached)
            {
                new DependencyChecker(typeof(InjectAttribute)).CheckDependencies(
                    w, audio, gamedata, clientgame, network, mapstorage, getfile,
                    config3d, mapManipulator, terrainDrawer, the3d, exit,
                    localplayerposition, worldfeatures, physics, mapgenerator,
                    internetgamefactory, blockdrawertorch, playerdrawer,
                    map, w.login, shadowsfull, shadowssimple, terrainChunkDrawer);
            }
        }
        #region IInternetGameFactory Members
        public void NewInternetGame()
        {
            MakeGame(false);
        }
        #endregion
        GMFortressLogic clientgame;
        InfiniteMapChunked map;
        ShadowsSimple shadowssimple;
        Shadows shadowsfull;
        WeaponBlockInfo weapon;
        public bool fullshadows = true;
        void UseShadowsSimple()
        {
            clientgame.shadows = shadowssimple;
            //map.shadows = clientgame.shadows;
            weapon.shadows = clientgame.shadows;
        }
        void UseShadowsFull()
        {
            clientgame.shadows = shadowsfull;
            //map.shadows = clientgame.shadows;
            weapon.shadows = clientgame.shadows;
        }
        #region ICurrentShadows Members
        public bool ShadowsFull
        {
            get
            {
                return fullshadows;
            }
            set
            {
                if (value && !fullshadows)
                {
                    UseShadowsFull();
                }
                if (!value && fullshadows)
                {
                    UseShadowsSimple();
                }
                fullshadows = value;
            }
        }
        #endregion
        public void ServerThread()
        {
            Server s = FortressModeServerFactory.create(true);
            s.Start();
            for (; ; )
            {
                s.Process();
                Thread.Sleep(1);
                if (exit != null && exit.exit) { return; }
            }
        }
    }
}

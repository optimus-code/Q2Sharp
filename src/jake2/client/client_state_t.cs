using J2N.IO;
using Jake2.Game;
using Jake2.Render;
using Jake2.Sound;
using Jake2.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jake2.Client
{
    public class client_state_t
    {
        public client_state_t()
        {
            for (int n = 0; n < Defines.CMD_BACKUP; n++)
                cmds[n] = new usercmd_t();
            for (int i = 0; i < frames.Length; i++)
            {
                frames[i] = new frame_t();
            }

            for (int n = 0; n < Defines.MAX_CONFIGSTRINGS; n++)
                configstrings[n] = new string("");
            for (int n = 0; n < Defines.MAX_CLIENTS; n++)
                clientinfo[n] = new clientinfo_t();
        }

        public int timeoutcount;
        public int timedemo_frames;
        public int timedemo_start;
        public bool refresh_prepped;
        public bool sound_prepped;
        public bool force_refdef;
        public int parse_entities;
        public usercmd_t cmd = new usercmd_t();
        public usercmd_t[] cmds = new usercmd_t[Defines.CMD_BACKUP];
        public int[] cmd_time = new int[Defines.CMD_BACKUP];
        public short[][] predicted_origins = Lib.CreateJaggedArray<short[][]>( Defines.CMD_BACKUP, 3 );
        public float predicted_step;
        public int predicted_step_time;
        public float[] predicted_origin = new float[]{0, 0, 0};
        public float[] predicted_angles = new float[]{0, 0, 0};
        public float[] prediction_error = new float[]{0, 0, 0};
        public frame_t frame = new frame_t();
        public int surpressCount;
        public frame_t[] frames = new frame_t[Defines.UPDATE_BACKUP];
        public float[] viewangles = new float[]{0, 0, 0};
        public int time;
        public float lerpfrac;
        public refdef_t refdef = new refdef_t();
        public float[] v_forward = new float[]{0, 0, 0};
        public float[] v_right = new float[]{0, 0, 0};
        public float[] v_up = new float[]{0, 0, 0};
        public string layout = "";
        public int[] inventory = new int[Defines.MAX_ITEMS];
        public ByteBuffer cinematic_file;
        public int cinematictime;
        public int cinematicframe;
        public byte[] cinematicpalette = new byte[768];
        public bool cinematicpalette_active;
        public bool attractloop;
        public int servercount;
        public string gamedir = "";
        public int playernum;
        public String[] configstrings = new string[Defines.MAX_CONFIGSTRINGS];
        public model_t[] model_draw = new model_t[Defines.MAX_MODELS];
        public cmodel_t[] model_clip = new cmodel_t[Defines.MAX_MODELS];
        public sfx_t[] sound_precache = new sfx_t[Defines.MAX_SOUNDS];
        public image_t[] image_precache = new image_t[Defines.MAX_IMAGES];
        public clientinfo_t[] clientinfo = new clientinfo_t[Defines.MAX_CLIENTS];
        public clientinfo_t baseclientinfo = new clientinfo_t();
    }
}
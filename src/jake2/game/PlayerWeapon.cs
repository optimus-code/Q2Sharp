using Q2Sharp.Game.Monsters;
using Q2Sharp.Util;
using System;

namespace Q2Sharp.Game
{
	public class PlayerWeapon
	{
		public static EntThinkAdapter Weapon_Grenade = new AnonymousEntThinkAdapter();
		private sealed class AnonymousEntThinkAdapter : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_Grenade";
			}

			public override Boolean Think( edict_t ent )
			{
				if ( ( ent.client.newweapon != null ) && ( ent.client.weaponstate == Defines.WEAPON_READY ) )
				{
					ChangeWeapon( ent );
					return true;
				}

				if ( ent.client.weaponstate == Defines.WEAPON_ACTIVATING )
				{
					ent.client.weaponstate = Defines.WEAPON_READY;
					ent.client.ps.gunframe = 16;
					return true;
				}

				if ( ent.client.weaponstate == Defines.WEAPON_READY )
				{
					if ( ( ( ent.client.latched_buttons | ent.client.buttons ) & Defines.BUTTON_ATTACK ) != 0 )
					{
						ent.client.latched_buttons &= ~Defines.BUTTON_ATTACK;
						if ( 0 != ent.client.pers.inventory[ent.client.ammo_index] )
						{
							ent.client.ps.gunframe = 1;
							ent.client.weaponstate = Defines.WEAPON_FIRING;
							ent.client.grenade_time = 0;
						}
						else
						{
							if ( GameBase.level.time >= ent.pain_debounce_time )
							{
								GameBase.gi.Sound( ent, Defines.CHAN_VOICE, GameBase.gi.Soundindex( "weapons/noammo.wav" ), 1, Defines.ATTN_NORM, 0 );
								ent.pain_debounce_time = GameBase.level.time + 1;
							}

							NoAmmoWeaponChange( ent );
						}

						return true;
					}

					if ( ( ent.client.ps.gunframe == 29 ) || ( ent.client.ps.gunframe == 34 ) || ( ent.client.ps.gunframe == 39 ) || ( ent.client.ps.gunframe == 48 ) )
					{
						if ( ( Lib.Rand() & 15 ) != 0 )
							return true;
					}

					if ( ++ent.client.ps.gunframe > 48 )
						ent.client.ps.gunframe = 16;
					return true;
				}

				if ( ent.client.weaponstate == Defines.WEAPON_FIRING )
				{
					if ( ent.client.ps.gunframe == 5 )
						GameBase.gi.Sound( ent, Defines.CHAN_WEAPON, GameBase.gi.Soundindex( "weapons/hgrena1b.wav" ), 1, Defines.ATTN_NORM, 0 );
					if ( ent.client.ps.gunframe == 11 )
					{
						if ( 0 == ent.client.grenade_time )
						{
							ent.client.grenade_time = GameBase.level.time + Defines.GRENADE_TIMER + 0.2F;
							ent.client.weapon_sound = GameBase.gi.Soundindex( "weapons/hgrenc1b.wav" );
						}

						if ( !ent.client.grenade_blew_up && GameBase.level.time >= ent.client.grenade_time )
						{
							ent.client.weapon_sound = 0;
							Weapon_grenade_fire( ent, true );
							ent.client.grenade_blew_up = true;
						}

						if ( ( ent.client.buttons & Defines.BUTTON_ATTACK ) != 0 )
							return true;
						if ( ent.client.grenade_blew_up )
						{
							if ( GameBase.level.time >= ent.client.grenade_time )
							{
								ent.client.ps.gunframe = 15;
								ent.client.grenade_blew_up = false;
							}
							else
							{
								return true;
							}
						}
					}

					if ( ent.client.ps.gunframe == 12 )
					{
						ent.client.weapon_sound = 0;
						Weapon_grenade_fire( ent, false );
					}

					if ( ( ent.client.ps.gunframe == 15 ) && ( GameBase.level.time < ent.client.grenade_time ) )
						return true;
					ent.client.ps.gunframe++;
					if ( ent.client.ps.gunframe == 16 )
					{
						ent.client.grenade_time = 0;
						ent.client.weaponstate = Defines.WEAPON_READY;
					}
				}

				return true;
			}
		}

		public static EntThinkAdapter weapon_grenadelauncher_fire = new AnonymousEntThinkAdapter1();
		private sealed class AnonymousEntThinkAdapter1 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "weapon_grenadelauncher_fire";
			}

			public override Boolean Think( edict_t ent )
			{
				Single[] offset = new Single[] { 0, 0, 0 };
				Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 };
				Single[] start = new Single[] { 0, 0, 0 };
				var damage = 120;
				Single radius;
				radius = damage + 40;
				if ( is_quad )
					damage *= 4;
				Math3D.VectorSet( offset, 8, 8, ent.viewheight - 8 );
				Math3D.AngleVectors( ent.client.v_angle, forward, right, null );
				P_ProjectSource( ent.client, ent.s.origin, offset, forward, right, start );
				Math3D.VectorScale( forward, -2, ent.client.kick_origin );
				ent.client.kick_angles[0] = -1;
				GameWeapon.Fire_grenade( ent, start, forward, damage, 600, 2.5F, radius );
				GameBase.gi.WriteByte( Defines.svc_muzzleflash );
				GameBase.gi.WriteShort( ent.index );
				GameBase.gi.WriteByte( Defines.MZ_GRENADE | is_silenced );
				GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
				ent.client.ps.gunframe++;
				PlayerWeapon.PlayerNoise( ent, start, Defines.PNOISE_WEAPON );
				if ( 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) )
					ent.client.pers.inventory[ent.client.ammo_index]--;
				return true;
			}
		}

		public static EntThinkAdapter Weapon_GrenadeLauncher = new AnonymousEntThinkAdapter2();
		private sealed class AnonymousEntThinkAdapter2 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_GrenadeLauncher";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32[] pause_frames = new[] { 34, 51, 59, 0 };
				Int32[] fire_frames = new[] { 6, 0 };
				Weapon_Generic( ent, 5, 16, 59, 64, pause_frames, fire_frames, weapon_grenadelauncher_fire );
				return true;
			}
		}

		public static EntThinkAdapter Weapon_RocketLauncher_Fire = new AnonymousEntThinkAdapter3();
		private sealed class AnonymousEntThinkAdapter3 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_RocketLauncher_Fire";
			}

			public override Boolean Think( edict_t ent )
			{
				Single[] offset = new Single[] { 0, 0, 0 }, start = new Single[] { 0, 0, 0 };
				Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 };
				Int32 damage;
				Single damage_radius;
				Int32 radius_damage;
				damage = 100 + ( Int32 ) ( Lib.Random() * 20 );
				radius_damage = 120;
				damage_radius = 120;
				if ( is_quad )
				{
					damage *= 4;
					radius_damage *= 4;
				}

				Math3D.AngleVectors( ent.client.v_angle, forward, right, null );
				Math3D.VectorScale( forward, -2, ent.client.kick_origin );
				ent.client.kick_angles[0] = -1;
				Math3D.VectorSet( offset, 8, 8, ent.viewheight - 8 );
				P_ProjectSource( ent.client, ent.s.origin, offset, forward, right, start );
				GameWeapon.Fire_rocket( ent, start, forward, damage, 650, damage_radius, radius_damage );
				GameBase.gi.WriteByte( Defines.svc_muzzleflash );
				GameBase.gi.WriteShort( ent.index );
				GameBase.gi.WriteByte( Defines.MZ_ROCKET | is_silenced );
				GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
				ent.client.ps.gunframe++;
				PlayerWeapon.PlayerNoise( ent, start, Defines.PNOISE_WEAPON );
				if ( 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) )
					ent.client.pers.inventory[ent.client.ammo_index]--;
				return true;
			}
		}

		public static EntThinkAdapter Weapon_RocketLauncher = new AnonymousEntThinkAdapter4();
		private sealed class AnonymousEntThinkAdapter4 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_RocketLauncher";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32[] pause_frames = new[] { 25, 33, 42, 50, 0 };
				Int32[] fire_frames = new[] { 5, 0 };
				Weapon_Generic( ent, 4, 12, 50, 54, pause_frames, fire_frames, Weapon_RocketLauncher_Fire );
				return true;
			}
		}

		public static EntThinkAdapter Weapon_Blaster_Fire = new AnonymousEntThinkAdapter5();
		private sealed class AnonymousEntThinkAdapter5 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_Blaster_Fire";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32 damage;
				if ( GameBase.deathmatch.value != 0 )
					damage = 15;
				else
					damage = 10;
				Blaster_Fire( ent, Globals.vec3_origin, damage, false, Defines.EF_BLASTER );
				ent.client.ps.gunframe++;
				return true;
			}
		}

		public static EntThinkAdapter Weapon_Blaster = new AnonymousEntThinkAdapter6();
		private sealed class AnonymousEntThinkAdapter6 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_Blaster";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32[] pause_frames = new[] { 19, 32, 0 };
				Int32[] fire_frames = new[] { 5, 0 };
				Weapon_Generic( ent, 4, 8, 52, 55, pause_frames, fire_frames, Weapon_Blaster_Fire );
				return true;
			}
		}

		public static EntThinkAdapter Weapon_HyperBlaster_Fire = new AnonymousEntThinkAdapter7();
		private sealed class AnonymousEntThinkAdapter7 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_HyperBlaster_Fire";
			}

			public override Boolean Think( edict_t ent )
			{
				Single rotation;
				Single[] offset = new Single[] { 0, 0, 0 };
				Int32 effect;
				Int32 damage;
				ent.client.weapon_sound = GameBase.gi.Soundindex( "weapons/hyprbl1a.wav" );
				if ( 0 == ( ent.client.buttons & Defines.BUTTON_ATTACK ) )
				{
					ent.client.ps.gunframe++;
				}
				else
				{
					if ( 0 == ent.client.pers.inventory[ent.client.ammo_index] )
					{
						if ( GameBase.level.time >= ent.pain_debounce_time )
						{
							GameBase.gi.Sound( ent, Defines.CHAN_VOICE, GameBase.gi.Soundindex( "weapons/noammo.wav" ), 1, Defines.ATTN_NORM, 0 );
							ent.pain_debounce_time = GameBase.level.time + 1;
						}

						NoAmmoWeaponChange( ent );
					}
					else
					{
						rotation = ( Single ) ( ( ent.client.ps.gunframe - 5 ) * 2 * Math.PI / 6 );
						offset[0] = ( Single ) ( -4 * Math.Sin( rotation ) );
						offset[1] = 0F;
						offset[2] = ( Single ) ( 4 * Math.Cos( rotation ) );
						if ( ( ent.client.ps.gunframe == 6 ) || ( ent.client.ps.gunframe == 9 ) )
							effect = Defines.EF_HYPERBLASTER;
						else
							effect = 0;
						if ( GameBase.deathmatch.value != 0 )
							damage = 15;
						else
							damage = 20;
						Blaster_Fire( ent, offset, damage, true, effect );
						if ( 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) )
							ent.client.pers.inventory[ent.client.ammo_index]--;
						ent.client.anim_priority = Defines.ANIM_ATTACK;
						if ( ( ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
						{
							ent.s.frame = M_Player.FRAME_crattak1 - 1;
							ent.client.anim_end = M_Player.FRAME_crattak9;
						}
						else
						{
							ent.s.frame = M_Player.FRAME_attack1 - 1;
							ent.client.anim_end = M_Player.FRAME_attack8;
						}
					}

					ent.client.ps.gunframe++;
					if ( ent.client.ps.gunframe == 12 && 0 != ent.client.pers.inventory[ent.client.ammo_index] )
						ent.client.ps.gunframe = 6;
				}

				if ( ent.client.ps.gunframe == 12 )
				{
					GameBase.gi.Sound( ent, Defines.CHAN_AUTO, GameBase.gi.Soundindex( "weapons/hyprbd1a.wav" ), 1, Defines.ATTN_NORM, 0 );
					ent.client.weapon_sound = 0;
				}

				return true;
			}
		}

		public static EntThinkAdapter Weapon_HyperBlaster = new AnonymousEntThinkAdapter8();
		private sealed class AnonymousEntThinkAdapter8 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_HyperBlaster";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32[] pause_frames = new[] { 0 };
				Int32[] fire_frames = new[] { 6, 7, 8, 9, 10, 11, 0 };
				Weapon_Generic( ent, 5, 20, 49, 53, pause_frames, fire_frames, Weapon_HyperBlaster_Fire );
				return true;
			}
		}

		public static EntThinkAdapter Weapon_Machinegun = new AnonymousEntThinkAdapter9();
		private sealed class AnonymousEntThinkAdapter9 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_Machinegun";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32[] pause_frames = new[] { 23, 45, 0 };
				Int32[] fire_frames = new[] { 4, 5, 0 };
				Weapon_Generic( ent, 3, 5, 45, 49, pause_frames, fire_frames, Machinegun_Fire );
				return true;
			}
		}

		public static EntThinkAdapter Weapon_Chaingun = new AnonymousEntThinkAdapter10();
		private sealed class AnonymousEntThinkAdapter10 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_Chaingun";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32[] pause_frames = new[] { 38, 43, 51, 61, 0 };
				Int32[] fire_frames = new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 0 };
				Weapon_Generic( ent, 4, 31, 61, 64, pause_frames, fire_frames, Chaingun_Fire );
				return true;
			}
		}

		public static EntThinkAdapter weapon_shotgun_fire = new AnonymousEntThinkAdapter11();
		private sealed class AnonymousEntThinkAdapter11 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "weapon_shotgun_fire";
			}

			public override Boolean Think( edict_t ent )
			{
				Single[] start = new Single[] { 0, 0, 0 };
				Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 };
				Single[] offset = new Single[] { 0, 0, 0 };
				var damage = 4;
				var kick = 8;
				if ( ent.client.ps.gunframe == 9 )
				{
					ent.client.ps.gunframe++;
					return true;
				}

				Math3D.AngleVectors( ent.client.v_angle, forward, right, null );
				Math3D.VectorScale( forward, -2, ent.client.kick_origin );
				ent.client.kick_angles[0] = -2;
				Math3D.VectorSet( offset, 0, 8, ent.viewheight - 8 );
				P_ProjectSource( ent.client, ent.s.origin, offset, forward, right, start );
				if ( is_quad )
				{
					damage *= 4;
					kick *= 4;
				}

				if ( GameBase.deathmatch.value != 0 )
					GameWeapon.Fire_shotgun( ent, start, forward, damage, kick, 500, 500, Defines.DEFAULT_DEATHMATCH_SHOTGUN_COUNT, Defines.MOD_SHOTGUN );
				else
					GameWeapon.Fire_shotgun( ent, start, forward, damage, kick, 500, 500, Defines.DEFAULT_SHOTGUN_COUNT, Defines.MOD_SHOTGUN );
				GameBase.gi.WriteByte( Defines.svc_muzzleflash );
				GameBase.gi.WriteShort( ent.index );
				GameBase.gi.WriteByte( Defines.MZ_SHOTGUN | is_silenced );
				GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
				ent.client.ps.gunframe++;
				PlayerWeapon.PlayerNoise( ent, start, Defines.PNOISE_WEAPON );
				if ( 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) )
					ent.client.pers.inventory[ent.client.ammo_index]--;
				return true;
			}
		}

		public static EntThinkAdapter Weapon_Shotgun = new AnonymousEntThinkAdapter12();
		private sealed class AnonymousEntThinkAdapter12 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_Shotgun";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32[] pause_frames = new[] { 22, 28, 34, 0 };
				Int32[] fire_frames = new[] { 8, 9, 0 };
				Weapon_Generic( ent, 7, 18, 36, 39, pause_frames, fire_frames, weapon_shotgun_fire );
				return true;
			}
		}

		public static EntThinkAdapter weapon_supershotgun_fire = new AnonymousEntThinkAdapter13();
		private sealed class AnonymousEntThinkAdapter13 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "weapon_supershotgun_fire";
			}

			public override Boolean Think( edict_t ent )
			{
				Single[] start = new Single[] { 0, 0, 0 };
				Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 };
				Single[] offset = new Single[] { 0, 0, 0 };
				Single[] v = new Single[] { 0, 0, 0 };
				var damage = 6;
				var kick = 12;
				Math3D.AngleVectors( ent.client.v_angle, forward, right, null );
				Math3D.VectorScale( forward, -2, ent.client.kick_origin );
				ent.client.kick_angles[0] = -2;
				Math3D.VectorSet( offset, 0, 8, ent.viewheight - 8 );
				P_ProjectSource( ent.client, ent.s.origin, offset, forward, right, start );
				if ( is_quad )
				{
					damage *= 4;
					kick *= 4;
				}

				v[Defines.PITCH] = ent.client.v_angle[Defines.PITCH];
				v[Defines.YAW] = ent.client.v_angle[Defines.YAW] - 5;
				v[Defines.ROLL] = ent.client.v_angle[Defines.ROLL];
				Math3D.AngleVectors( v, forward, null, null );
				GameWeapon.Fire_shotgun( ent, start, forward, damage, kick, Defines.DEFAULT_SHOTGUN_HSPREAD, Defines.DEFAULT_SHOTGUN_VSPREAD, Defines.DEFAULT_SSHOTGUN_COUNT / 2, Defines.MOD_SSHOTGUN );
				v[Defines.YAW] = ent.client.v_angle[Defines.YAW] + 5;
				Math3D.AngleVectors( v, forward, null, null );
				GameWeapon.Fire_shotgun( ent, start, forward, damage, kick, Defines.DEFAULT_SHOTGUN_HSPREAD, Defines.DEFAULT_SHOTGUN_VSPREAD, Defines.DEFAULT_SSHOTGUN_COUNT / 2, Defines.MOD_SSHOTGUN );
				GameBase.gi.WriteByte( Defines.svc_muzzleflash );
				GameBase.gi.WriteShort( ent.index );
				GameBase.gi.WriteByte( Defines.MZ_SSHOTGUN | is_silenced );
				GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
				ent.client.ps.gunframe++;
				PlayerWeapon.PlayerNoise( ent, start, Defines.PNOISE_WEAPON );
				if ( 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) )
					ent.client.pers.inventory[ent.client.ammo_index] -= 2;
				return true;
			}
		}

		public static EntThinkAdapter Weapon_SuperShotgun = new AnonymousEntThinkAdapter14();
		private sealed class AnonymousEntThinkAdapter14 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_SuperShotgun";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32[] pause_frames = new[] { 29, 42, 57, 0 };
				Int32[] fire_frames = new[] { 7, 0 };
				Weapon_Generic( ent, 6, 17, 57, 61, pause_frames, fire_frames, weapon_supershotgun_fire );
				return true;
			}
		}

		public static EntThinkAdapter weapon_railgun_fire = new AnonymousEntThinkAdapter15();
		private sealed class AnonymousEntThinkAdapter15 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "weapon_railgun_fire";
			}

			public override Boolean Think( edict_t ent )
			{
				Single[] start = new Single[] { 0, 0, 0 };
				Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 };
				Single[] offset = new Single[] { 0, 0, 0 };
				Int32 damage;
				Int32 kick;
				if ( GameBase.deathmatch.value != 0 )
				{
					damage = 100;
					kick = 200;
				}
				else
				{
					damage = 150;
					kick = 250;
				}

				if ( is_quad )
				{
					damage *= 4;
					kick *= 4;
				}

				Math3D.AngleVectors( ent.client.v_angle, forward, right, null );
				Math3D.VectorScale( forward, -3, ent.client.kick_origin );
				ent.client.kick_angles[0] = -3;
				Math3D.VectorSet( offset, 0, 7, ent.viewheight - 8 );
				P_ProjectSource( ent.client, ent.s.origin, offset, forward, right, start );
				GameWeapon.Fire_rail( ent, start, forward, damage, kick );
				GameBase.gi.WriteByte( Defines.svc_muzzleflash );
				GameBase.gi.WriteShort( ent.index );
				GameBase.gi.WriteByte( Defines.MZ_RAILGUN | is_silenced );
				GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
				ent.client.ps.gunframe++;
				PlayerWeapon.PlayerNoise( ent, start, Defines.PNOISE_WEAPON );
				if ( 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) )
					ent.client.pers.inventory[ent.client.ammo_index]--;
				return true;
			}
		}

		public static EntThinkAdapter Weapon_Railgun = new AnonymousEntThinkAdapter16();
		private sealed class AnonymousEntThinkAdapter16 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_Railgun";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32[] pause_frames = new[] { 56, 0 };
				Int32[] fire_frames = new[] { 4, 0 };
				Weapon_Generic( ent, 3, 18, 56, 61, pause_frames, fire_frames, weapon_railgun_fire );
				return true;
			}
		}

		public static EntThinkAdapter weapon_bfg_fire = new AnonymousEntThinkAdapter17();
		private sealed class AnonymousEntThinkAdapter17 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "weapon_bfg_fire";
			}

			public override Boolean Think( edict_t ent )
			{
				Single[] offset = new Single[] { 0, 0, 0 }, start = new Single[] { 0, 0, 0 };
				Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 };
				Int32 damage;
				Single damage_radius = 1000;
				if ( GameBase.deathmatch.value != 0 )
					damage = 200;
				else
					damage = 500;
				if ( ent.client.ps.gunframe == 9 )
				{
					GameBase.gi.WriteByte( Defines.svc_muzzleflash );
					GameBase.gi.WriteShort( ent.index );
					GameBase.gi.WriteByte( Defines.MZ_BFG | is_silenced );
					GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
					ent.client.ps.gunframe++;
					PlayerWeapon.PlayerNoise( ent, start, Defines.PNOISE_WEAPON );
					return true;
				}

				if ( ent.client.pers.inventory[ent.client.ammo_index] < 50 )
				{
					ent.client.ps.gunframe++;
					return true;
				}

				if ( is_quad )
					damage *= 4;
				Math3D.AngleVectors( ent.client.v_angle, forward, right, null );
				Math3D.VectorScale( forward, -2, ent.client.kick_origin );
				ent.client.v_dmg_pitch = -40;
				ent.client.v_dmg_roll = Lib.Crandom() * 8;
				ent.client.v_dmg_time = GameBase.level.time + Defines.DAMAGE_TIME;
				Math3D.VectorSet( offset, 8, 8, ent.viewheight - 8 );
				P_ProjectSource( ent.client, ent.s.origin, offset, forward, right, start );
				GameWeapon.Fire_bfg( ent, start, forward, damage, 400, damage_radius );
				ent.client.ps.gunframe++;
				PlayerWeapon.PlayerNoise( ent, start, Defines.PNOISE_WEAPON );
				if ( 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) )
					ent.client.pers.inventory[ent.client.ammo_index] -= 50;
				return true;
			}
		}

		public static EntThinkAdapter Weapon_BFG = new AnonymousEntThinkAdapter18();
		private sealed class AnonymousEntThinkAdapter18 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Weapon_BFG";
			}

			public override Boolean Think( edict_t ent )
			{
				Weapon_Generic( ent, 8, 32, 55, 58, pause_frames, fire_frames, weapon_bfg_fire );
				return true;
			}
		}

		public static Boolean is_quad;
		public static Byte is_silenced;
		public static ItemUseAdapter Use_Weapon = new AnonymousItemUseAdapter();
		private sealed class AnonymousItemUseAdapter : ItemUseAdapter
		{
			public override String GetID( )
			{
				return "Use_Weapon";
			}

			public override void Use( edict_t ent, gitem_t item )
			{
				Int32 ammo_index;
				gitem_t ammo_item;
				if ( item == ent.client.pers.weapon )
					return;
				if ( item.ammo != null && 0 == GameBase.g_select_empty.value && 0 == ( item.flags & Defines.IT_AMMO ) )
				{
					ammo_item = GameItems.FindItem( item.ammo );
					ammo_index = GameItems.ITEM_INDEX( ammo_item );
					if ( 0 == ent.client.pers.inventory[ammo_index] )
					{
						GameBase.gi.Cprintf( ent, Defines.PRINT_HIGH, "No " + ammo_item.pickup_name + " for " + item.pickup_name + ".\\n" );
						return;
					}

					if ( ent.client.pers.inventory[ammo_index] < item.quantity )
					{
						GameBase.gi.Cprintf( ent, Defines.PRINT_HIGH, "Not enough " + ammo_item.pickup_name + " for " + item.pickup_name + ".\\n" );
						return;
					}
				}

				ent.client.newweapon = item;
			}
		}

		public static ItemDropAdapter Drop_Weapon = new AnonymousItemDropAdapter();
		private sealed class AnonymousItemDropAdapter : ItemDropAdapter
		{
			public override String GetID( )
			{
				return "Drop_Weapon";
			}

			public override void Drop( edict_t ent, gitem_t item )
			{
				Int32 index;
				if ( 0 != ( ( Int32 ) ( GameBase.dmflags.value ) & Defines.DF_WEAPONS_STAY ) )
					return;
				index = GameItems.ITEM_INDEX( item );
				if ( ( ( item == ent.client.pers.weapon ) || ( item == ent.client.newweapon ) ) && ( ent.client.pers.inventory[index] == 1 ) )
				{
					GameBase.gi.Cprintf( ent, Defines.PRINT_HIGH, "Can't drop current weapon\\n" );
					return;
				}

				GameItems.Drop_Item( ent, item );
				ent.client.pers.inventory[index]--;
			}
		}

		public static EntThinkAdapter Machinegun_Fire = new AnonymousEntThinkAdapter19();
		private sealed class AnonymousEntThinkAdapter19 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Machinegun_Fire";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32 i;
				Single[] start = new Single[] { 0, 0, 0 };
				Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 };
				Single[] angles = new Single[] { 0, 0, 0 };
				var damage = 8;
				var kick = 2;
				Single[] offset = new Single[] { 0, 0, 0 };
				if ( 0 == ( ent.client.buttons & Defines.BUTTON_ATTACK ) )
				{
					ent.client.machinegun_shots = 0;
					ent.client.ps.gunframe++;
					return true;
				}

				if ( ent.client.ps.gunframe == 5 )
					ent.client.ps.gunframe = 4;
				else
					ent.client.ps.gunframe = 5;
				if ( ent.client.pers.inventory[ent.client.ammo_index] < 1 )
				{
					ent.client.ps.gunframe = 6;
					if ( GameBase.level.time >= ent.pain_debounce_time )
					{
						GameBase.gi.Sound( ent, Defines.CHAN_VOICE, GameBase.gi.Soundindex( "weapons/noammo.wav" ), 1, Defines.ATTN_NORM, 0 );
						ent.pain_debounce_time = GameBase.level.time + 1;
					}

					NoAmmoWeaponChange( ent );
					return true;
				}

				if ( is_quad )
				{
					damage *= 4;
					kick *= 4;
				}

				for ( i = 1; i < 3; i++ )
				{
					ent.client.kick_origin[i] = Lib.Crandom() * 0.35F;
					ent.client.kick_angles[i] = Lib.Crandom() * 0.7F;
				}

				ent.client.kick_origin[0] = Lib.Crandom() * 0.35F;
				ent.client.kick_angles[0] = ent.client.machinegun_shots * -1.5F;
				if ( 0 == GameBase.deathmatch.value )
				{
					ent.client.machinegun_shots++;
					if ( ent.client.machinegun_shots > 9 )
						ent.client.machinegun_shots = 9;
				}

				Math3D.VectorAdd( ent.client.v_angle, ent.client.kick_angles, angles );
				Math3D.AngleVectors( angles, forward, right, null );
				Math3D.VectorSet( offset, 0, 8, ent.viewheight - 8 );
				P_ProjectSource( ent.client, ent.s.origin, offset, forward, right, start );
				GameWeapon.Fire_bullet( ent, start, forward, damage, kick, Defines.DEFAULT_BULLET_HSPREAD, Defines.DEFAULT_BULLET_VSPREAD, Defines.MOD_MACHINEGUN );
				GameBase.gi.WriteByte( Defines.svc_muzzleflash );
				GameBase.gi.WriteShort( ent.index );
				GameBase.gi.WriteByte( Defines.MZ_MACHINEGUN | is_silenced );
				GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
				PlayerWeapon.PlayerNoise( ent, start, Defines.PNOISE_WEAPON );
				if ( 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) )
					ent.client.pers.inventory[ent.client.ammo_index]--;
				ent.client.anim_priority = Defines.ANIM_ATTACK;
				if ( ( ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
				{
					ent.s.frame = M_Player.FRAME_crattak1 - ( Int32 ) ( Lib.Random() + 0.25 );
					ent.client.anim_end = M_Player.FRAME_crattak9;
				}
				else
				{
					ent.s.frame = M_Player.FRAME_attack1 - ( Int32 ) ( Lib.Random() + 0.25 );
					ent.client.anim_end = M_Player.FRAME_attack8;
				}

				return true;
			}
		}

		public static EntThinkAdapter Chaingun_Fire = new AnonymousEntThinkAdapter20();
		private sealed class AnonymousEntThinkAdapter20 : EntThinkAdapter
		{

			public override String GetID( )
			{
				return "Chaingun_Fire";
			}

			public override Boolean Think( edict_t ent )
			{
				Int32 i;
				Int32 shots;
				Single[] start = new Single[] { 0, 0, 0 };
				Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 }, up = new Single[] { 0, 0, 0 };
				Single r, u;
				Single[] offset = new Single[] { 0, 0, 0 };
				Int32 damage;
				var kick = 2;
				if ( GameBase.deathmatch.value != 0 )
					damage = 6;
				else
					damage = 8;
				if ( ent.client.ps.gunframe == 5 )
					GameBase.gi.Sound( ent, Defines.CHAN_AUTO, GameBase.gi.Soundindex( "weapons/chngnu1a.wav" ), 1, Defines.ATTN_IDLE, 0 );
				if ( ( ent.client.ps.gunframe == 14 ) && 0 == ( ent.client.buttons & Defines.BUTTON_ATTACK ) )
				{
					ent.client.ps.gunframe = 32;
					ent.client.weapon_sound = 0;
					return true;
				}
				else if ( ( ent.client.ps.gunframe == 21 ) && ( ent.client.buttons & Defines.BUTTON_ATTACK ) != 0 && 0 != ent.client.pers.inventory[ent.client.ammo_index] )
				{
					ent.client.ps.gunframe = 15;
				}
				else
				{
					ent.client.ps.gunframe++;
				}

				if ( ent.client.ps.gunframe == 22 )
				{
					ent.client.weapon_sound = 0;
					GameBase.gi.Sound( ent, Defines.CHAN_AUTO, GameBase.gi.Soundindex( "weapons/chngnd1a.wav" ), 1, Defines.ATTN_IDLE, 0 );
				}
				else
				{
					ent.client.weapon_sound = GameBase.gi.Soundindex( "weapons/chngnl1a.wav" );
				}

				ent.client.anim_priority = Defines.ANIM_ATTACK;
				if ( ( ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
				{
					ent.s.frame = M_Player.FRAME_crattak1 - ( ent.client.ps.gunframe & 1 );
					ent.client.anim_end = M_Player.FRAME_crattak9;
				}
				else
				{
					ent.s.frame = M_Player.FRAME_attack1 - ( ent.client.ps.gunframe & 1 );
					ent.client.anim_end = M_Player.FRAME_attack8;
				}

				if ( ent.client.ps.gunframe <= 9 )
					shots = 1;
				else if ( ent.client.ps.gunframe <= 14 )
				{
					if ( ( ent.client.buttons & Defines.BUTTON_ATTACK ) != 0 )
						shots = 2;
					else
						shots = 1;
				}
				else
					shots = 3;
				if ( ent.client.pers.inventory[ent.client.ammo_index] < shots )
					shots = ent.client.pers.inventory[ent.client.ammo_index];
				if ( 0 == shots )
				{
					if ( GameBase.level.time >= ent.pain_debounce_time )
					{
						GameBase.gi.Sound( ent, Defines.CHAN_VOICE, GameBase.gi.Soundindex( "weapons/noammo.wav" ), 1, Defines.ATTN_NORM, 0 );
						ent.pain_debounce_time = GameBase.level.time + 1;
					}

					NoAmmoWeaponChange( ent );
					return true;
				}

				if ( is_quad )
				{
					damage *= 4;
					kick *= 4;
				}

				for ( i = 0; i < 3; i++ )
				{
					ent.client.kick_origin[i] = Lib.Crandom() * 0.35F;
					ent.client.kick_angles[i] = Lib.Crandom() * 0.7F;
				}

				for ( i = 0; i < shots; i++ )
				{
					Math3D.AngleVectors( ent.client.v_angle, forward, right, up );
					r = 7 + Lib.Crandom() * 4;
					u = Lib.Crandom() * 4;
					Math3D.VectorSet( offset, 0, r, u + ent.viewheight - 8 );
					P_ProjectSource( ent.client, ent.s.origin, offset, forward, right, start );
					GameWeapon.Fire_bullet( ent, start, forward, damage, kick, Defines.DEFAULT_BULLET_HSPREAD, Defines.DEFAULT_BULLET_VSPREAD, Defines.MOD_CHAINGUN );
				}

				GameBase.gi.WriteByte( Defines.svc_muzzleflash );
				GameBase.gi.WriteShort( ent.index );
				GameBase.gi.WriteByte( ( Defines.MZ_CHAINGUN1 + shots - 1 ) | is_silenced );
				GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
				PlayerWeapon.PlayerNoise( ent, start, Defines.PNOISE_WEAPON );
				if ( 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) )
					ent.client.pers.inventory[ent.client.ammo_index] -= shots;
				return true;
			}
		}

		public static Int32[] pause_frames = new[] { 39, 45, 50, 55, 0 };
		public static Int32[] fire_frames = new[] { 9, 17, 0 };
		public static EntInteractAdapter Pickup_Weapon = new AnonymousEntInteractAdapter();
		private sealed class AnonymousEntInteractAdapter : EntInteractAdapter
		{

			public override String GetID( )
			{
				return "Pickup_Weapon";
			}

			public override Boolean Interact( edict_t ent, edict_t other )
			{
				Int32 index;
				gitem_t ammo;
				index = GameItems.ITEM_INDEX( ent.item );
				if ( ( ( ( Int32 ) ( GameBase.dmflags.value ) & Defines.DF_WEAPONS_STAY ) != 0 || GameBase.coop.value != 0 ) && 0 != other.client.pers.inventory[index] )
				{
					if ( 0 == ( ent.spawnflags & ( Defines.DROPPED_ITEM | Defines.DROPPED_PLAYER_ITEM ) ) )
						return false;
				}

				other.client.pers.inventory[index]++;
				if ( 0 == ( ent.spawnflags & Defines.DROPPED_ITEM ) )
				{
					ammo = GameItems.FindItem( ent.item.ammo );
					if ( ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) != 0 )
						GameItems.Add_Ammo( other, ammo, 1000 );
					else
						GameItems.Add_Ammo( other, ammo, ammo.quantity );
					if ( 0 == ( ent.spawnflags & Defines.DROPPED_PLAYER_ITEM ) )
					{
						if ( GameBase.deathmatch.value != 0 )
						{
							if ( ( ( Int32 ) ( GameBase.dmflags.value ) & Defines.DF_WEAPONS_STAY ) != 0 )
								ent.flags |= Defines.FL_RESPAWN;
							else
								GameItems.SetRespawn( ent, 30 );
						}

						if ( GameBase.coop.value != 0 )
							ent.flags |= Defines.FL_RESPAWN;
					}
				}

				if ( other.client.pers.weapon != ent.item && ( other.client.pers.inventory[index] == 1 ) && ( 0 == GameBase.deathmatch.value || other.client.pers.weapon == GameItems.FindItem( "blaster" ) ) )
					other.client.newweapon = ent.item;
				return true;
			}
		}

		public static void P_ProjectSource( gclient_t client, Single[] point, Single[] distance, Single[] forward, Single[] right, Single[] result )
		{
			Single[] _distance = new Single[] { 0, 0, 0 };
			Math3D.VectorCopy( distance, _distance );
			if ( client.pers.hand == Defines.LEFT_HANDED )
				_distance[1] *= -1;
			else if ( client.pers.hand == Defines.CENTER_HANDED )
				_distance[1] = 0;
			Math3D.G_ProjectSource( point, _distance, forward, right, result );
		}

		public static void ChangeWeapon( edict_t ent )
		{
			Int32 i;
			if ( ent.client.grenade_time != 0 )
			{
				ent.client.grenade_time = GameBase.level.time;
				ent.client.weapon_sound = 0;
				Weapon_grenade_fire( ent, false );
				ent.client.grenade_time = 0;
			}

			ent.client.pers.lastweapon = ent.client.pers.weapon;
			ent.client.pers.weapon = ent.client.newweapon;
			ent.client.newweapon = null;
			ent.client.machinegun_shots = 0;
			if ( ent.s.modelindex == 255 )
			{
				if ( ent.client.pers.weapon != null )
					i = ( ( ent.client.pers.weapon.weapmodel & 0xff ) << 8 );
				else
					i = 0;
				ent.s.skinnum = unchecked( ( ent.index - 1 ) | i );
			}

			if ( ent.client.pers.weapon != null && ent.client.pers.weapon.ammo != null )
				ent.client.ammo_index = GameItems.ITEM_INDEX( GameItems.FindItem( ent.client.pers.weapon.ammo ) );
			else
				ent.client.ammo_index = 0;
			if ( ent.client.pers.weapon == null )
			{
				ent.client.ps.gunindex = 0;
				return;
			}

			ent.client.weaponstate = Defines.WEAPON_ACTIVATING;
			ent.client.ps.gunframe = 0;
			ent.client.ps.gunindex = GameBase.gi.Modelindex( ent.client.pers.weapon.view_model );
			ent.client.anim_priority = Defines.ANIM_PAIN;
			if ( ( ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
			{
				ent.s.frame = M_Player.FRAME_crpain1;
				ent.client.anim_end = M_Player.FRAME_crpain4;
			}
			else
			{
				ent.s.frame = M_Player.FRAME_pain301;
				ent.client.anim_end = M_Player.FRAME_pain304;
			}
		}

		public static void NoAmmoWeaponChange( edict_t ent )
		{
			if ( 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "slugs" ) )] && 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "railgun" ) )] )
			{
				ent.client.newweapon = GameItems.FindItem( "railgun" );
				return;
			}

			if ( 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "cells" ) )] && 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "hyperblaster" ) )] )
			{
				ent.client.newweapon = GameItems.FindItem( "hyperblaster" );
				return;
			}

			if ( 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "bullets" ) )] && 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "chaingun" ) )] )
			{
				ent.client.newweapon = GameItems.FindItem( "chaingun" );
				return;
			}

			if ( 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "bullets" ) )] && 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "machinegun" ) )] )
			{
				ent.client.newweapon = GameItems.FindItem( "machinegun" );
				return;
			}

			if ( ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "shells" ) )] > 1 && 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "super shotgun" ) )] )
			{
				ent.client.newweapon = GameItems.FindItem( "super shotgun" );
				return;
			}

			if ( 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "shells" ) )] && 0 != ent.client.pers.inventory[GameItems.ITEM_INDEX( GameItems.FindItem( "shotgun" ) )] )
			{
				ent.client.newweapon = GameItems.FindItem( "shotgun" );
				return;
			}

			ent.client.newweapon = GameItems.FindItem( "blaster" );
		}

		public static void Think_Weapon( edict_t ent )
		{
			if ( ent.health < 1 )
			{
				ent.client.newweapon = null;
				ChangeWeapon( ent );
			}

			if ( null != ent.client.pers.weapon && null != ent.client.pers.weapon.weaponthink )
			{
				is_quad = ( ent.client.quad_framenum > GameBase.level.framenum );
				if ( ent.client.silencer_shots != 0 )
					is_silenced = ( Byte ) Defines.MZ_SILENCED;
				else
					is_silenced = 0;
				ent.client.pers.weapon.weaponthink.Think( ent );
			}
		}

		public static void Weapon_Generic( edict_t ent, Int32 FRAME_ACTIVATE_LAST, Int32 FRAME_FIRE_LAST, Int32 FRAME_IDLE_LAST, Int32 FRAME_DEACTIVATE_LAST, Int32[] pause_frames, Int32[] fire_frames, EntThinkAdapter fire )
		{
			var FRAME_FIRE_FIRST = ( FRAME_ACTIVATE_LAST + 1 );
			var FRAME_IDLE_FIRST = ( FRAME_FIRE_LAST + 1 );
			var FRAME_DEACTIVATE_FIRST = ( FRAME_IDLE_LAST + 1 );
			Int32 n;
			if ( ent.deadflag != 0 || ent.s.modelindex != 255 )
			{
				return;
			}

			if ( ent.client.weaponstate == Defines.WEAPON_DROPPING )
			{
				if ( ent.client.ps.gunframe == FRAME_DEACTIVATE_LAST )
				{
					ChangeWeapon( ent );
					return;
				}
				else if ( ( FRAME_DEACTIVATE_LAST - ent.client.ps.gunframe ) == 4 )
				{
					ent.client.anim_priority = Defines.ANIM_REVERSE;
					if ( ( ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
					{
						ent.s.frame = M_Player.FRAME_crpain4 + 1;
						ent.client.anim_end = M_Player.FRAME_crpain1;
					}
					else
					{
						ent.s.frame = M_Player.FRAME_pain304 + 1;
						ent.client.anim_end = M_Player.FRAME_pain301;
					}
				}

				ent.client.ps.gunframe++;
				return;
			}

			if ( ent.client.weaponstate == Defines.WEAPON_ACTIVATING )
			{
				if ( ent.client.ps.gunframe == FRAME_ACTIVATE_LAST )
				{
					ent.client.weaponstate = Defines.WEAPON_READY;
					ent.client.ps.gunframe = FRAME_IDLE_FIRST;
					return;
				}

				ent.client.ps.gunframe++;
				return;
			}

			if ( ( ent.client.newweapon != null ) && ( ent.client.weaponstate != Defines.WEAPON_FIRING ) )
			{
				ent.client.weaponstate = Defines.WEAPON_DROPPING;
				ent.client.ps.gunframe = FRAME_DEACTIVATE_FIRST;
				if ( ( FRAME_DEACTIVATE_LAST - FRAME_DEACTIVATE_FIRST ) < 4 )
				{
					ent.client.anim_priority = Defines.ANIM_REVERSE;
					if ( ( ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
					{
						ent.s.frame = M_Player.FRAME_crpain4 + 1;
						ent.client.anim_end = M_Player.FRAME_crpain1;
					}
					else
					{
						ent.s.frame = M_Player.FRAME_pain304 + 1;
						ent.client.anim_end = M_Player.FRAME_pain301;
					}
				}

				return;
			}

			if ( ent.client.weaponstate == Defines.WEAPON_READY )
			{
				if ( ( ( ent.client.latched_buttons | ent.client.buttons ) & Defines.BUTTON_ATTACK ) != 0 )
				{
					ent.client.latched_buttons &= ~Defines.BUTTON_ATTACK;
					if ( ( 0 == ent.client.ammo_index ) || ( ent.client.pers.inventory[ent.client.ammo_index] >= ent.client.pers.weapon.quantity ) )
					{
						ent.client.ps.gunframe = FRAME_FIRE_FIRST;
						ent.client.weaponstate = Defines.WEAPON_FIRING;
						ent.client.anim_priority = Defines.ANIM_ATTACK;
						if ( ( ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
						{
							ent.s.frame = M_Player.FRAME_crattak1 - 1;
							ent.client.anim_end = M_Player.FRAME_crattak9;
						}
						else
						{
							ent.s.frame = M_Player.FRAME_attack1 - 1;
							ent.client.anim_end = M_Player.FRAME_attack8;
						}
					}
					else
					{
						if ( GameBase.level.time >= ent.pain_debounce_time )
						{
							GameBase.gi.Sound( ent, Defines.CHAN_VOICE, GameBase.gi.Soundindex( "weapons/noammo.wav" ), 1, Defines.ATTN_NORM, 0 );
							ent.pain_debounce_time = GameBase.level.time + 1;
						}

						NoAmmoWeaponChange( ent );
					}
				}
				else
				{
					if ( ent.client.ps.gunframe == FRAME_IDLE_LAST )
					{
						ent.client.ps.gunframe = FRAME_IDLE_FIRST;
						return;
					}

					if ( pause_frames != null )
					{
						for ( n = 0; pause_frames[n] != 0; n++ )
						{
							if ( ent.client.ps.gunframe == pause_frames[n] )
							{
								if ( ( Lib.Rand() & 15 ) != 0 )
									return;
							}
						}
					}

					ent.client.ps.gunframe++;
					return;
				}
			}

			if ( ent.client.weaponstate == Defines.WEAPON_FIRING )
			{
				for ( n = 0; fire_frames[n] != 0; n++ )
				{
					if ( ent.client.ps.gunframe == fire_frames[n] )
					{
						if ( ent.client.quad_framenum > GameBase.level.framenum )
							GameBase.gi.Sound( ent, Defines.CHAN_ITEM, GameBase.gi.Soundindex( "items/damage3.wav" ), 1, Defines.ATTN_NORM, 0 );
						fire.Think( ent );
						break;
					}
				}

				if ( 0 == fire_frames[n] )
					ent.client.ps.gunframe++;
				if ( ent.client.ps.gunframe == FRAME_IDLE_FIRST + 1 )
					ent.client.weaponstate = Defines.WEAPON_READY;
			}
		}

		public static void Weapon_grenade_fire( edict_t ent, Boolean held )
		{
			Single[] offset = new Single[] { 0, 0, 0 };
			Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 };
			Single[] start = new Single[] { 0, 0, 0 };
			var damage = 125;
			Single timer;
			Int32 speed;
			Single radius;
			radius = damage + 40;
			if ( is_quad )
				damage *= 4;
			Math3D.VectorSet( offset, 8, 8, ent.viewheight - 8 );
			Math3D.AngleVectors( ent.client.v_angle, forward, right, null );
			P_ProjectSource( ent.client, ent.s.origin, offset, forward, right, start );
			timer = ent.client.grenade_time - GameBase.level.time;
			speed = ( Int32 ) ( Defines.GRENADE_MINSPEED + ( Defines.GRENADE_TIMER - timer ) * ( ( Defines.GRENADE_MAXSPEED - Defines.GRENADE_MINSPEED ) / Defines.GRENADE_TIMER ) );
			GameWeapon.Fire_grenade2( ent, start, forward, damage, speed, timer, radius, held );
			if ( 0 == ( ( Int32 ) GameBase.dmflags.value & Defines.DF_INFINITE_AMMO ) )
				ent.client.pers.inventory[ent.client.ammo_index]--;
			ent.client.grenade_time = GameBase.level.time + 1F;
			if ( ent.deadflag != 0 || ent.s.modelindex != 255 )
			{
				return;
			}

			if ( ent.health <= 0 )
				return;
			if ( ( ent.client.ps.pmove.pm_flags & pmove_t.PMF_DUCKED ) != 0 )
			{
				ent.client.anim_priority = Defines.ANIM_ATTACK;
				ent.s.frame = M_Player.FRAME_crattak1 - 1;
				ent.client.anim_end = M_Player.FRAME_crattak3;
			}
			else
			{
				ent.client.anim_priority = Defines.ANIM_REVERSE;
				ent.s.frame = M_Player.FRAME_wave08;
				ent.client.anim_end = M_Player.FRAME_wave01;
			}
		}

		public static void Blaster_Fire( edict_t ent, Single[] g_offset, Int32 damage, Boolean hyper, Int32 effect )
		{
			Single[] forward = new Single[] { 0, 0, 0 }, right = new Single[] { 0, 0, 0 };
			Single[] start = new Single[] { 0, 0, 0 };
			Single[] offset = new Single[] { 0, 0, 0 };
			if ( is_quad )
				damage *= 4;
			Math3D.AngleVectors( ent.client.v_angle, forward, right, null );
			Math3D.VectorSet( offset, 24, 8, ent.viewheight - 8 );
			Math3D.VectorAdd( offset, g_offset, offset );
			P_ProjectSource( ent.client, ent.s.origin, offset, forward, right, start );
			Math3D.VectorScale( forward, -2, ent.client.kick_origin );
			ent.client.kick_angles[0] = -1;
			GameWeapon.Fire_blaster( ent, start, forward, damage, 1000, effect, hyper );
			GameBase.gi.WriteByte( Defines.svc_muzzleflash );
			GameBase.gi.WriteShort( ent.index );
			if ( hyper )
				GameBase.gi.WriteByte( Defines.MZ_HYPERBLASTER | is_silenced );
			else
				GameBase.gi.WriteByte( Defines.MZ_BLASTER | is_silenced );
			GameBase.gi.Multicast( ent.s.origin, Defines.MULTICAST_PVS );
			PlayerWeapon.PlayerNoise( ent, start, Defines.PNOISE_WEAPON );
		}

		public static void PlayerNoise( edict_t who, Single[] where, Int32 type )
		{
			edict_t noise;
			if ( type == Defines.PNOISE_WEAPON )
			{
				if ( who.client.silencer_shots > 0 )
				{
					who.client.silencer_shots--;
					return;
				}
			}

			if ( GameBase.deathmatch.value != 0 )
				return;
			if ( ( who.flags & Defines.FL_NOTARGET ) != 0 )
				return;
			if ( who.mynoise == null )
			{
				noise = GameUtil.G_Spawn();
				noise.classname = "player_noise";
				Math3D.VectorSet( noise.mins, -8, -8, -8 );
				Math3D.VectorSet( noise.maxs, 8, 8, 8 );
				noise.owner = who;
				noise.svflags = Defines.SVF_NOCLIENT;
				who.mynoise = noise;
				noise = GameUtil.G_Spawn();
				noise.classname = "player_noise";
				Math3D.VectorSet( noise.mins, -8, -8, -8 );
				Math3D.VectorSet( noise.maxs, 8, 8, 8 );
				noise.owner = who;
				noise.svflags = Defines.SVF_NOCLIENT;
				who.mynoise2 = noise;
			}

			if ( type == Defines.PNOISE_SELF || type == Defines.PNOISE_WEAPON )
			{
				noise = who.mynoise;
				GameBase.level.sound_entity = noise;
				GameBase.level.sound_entity_framenum = GameBase.level.framenum;
			}
			else
			{
				noise = who.mynoise2;
				GameBase.level.sound2_entity = noise;
				GameBase.level.sound2_entity_framenum = GameBase.level.framenum;
			}

			Math3D.VectorCopy( where, noise.s.origin );
			Math3D.VectorSubtract( where, noise.maxs, noise.absmin );
			Math3D.VectorAdd( where, noise.maxs, noise.absmax );
			noise.teleport_time = GameBase.level.time;
			GameBase.gi.Linkentity( noise );
		}
	}
}
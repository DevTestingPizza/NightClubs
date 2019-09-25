using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace NightClub
{
    public class IplManager : BaseScript
    {
        public const int NIGHTCLUB_INTERIOR_ID = 271617;
        public static Vector3 interiorLocation = new Vector3(-1604.664f, -3012.583f, -78.000f);
        public IplManager()
        {
            if (!IsIplActive("ba_int_placement_ba_interior_0_dlc_int_01_ba_milo_"))
            {
                RequestIpl("ba_int_placement_ba_interior_0_dlc_int_01_ba_milo_");
            }
        }

        public static bool IsInInterior { get; private set; } = false;

        [Tick]
        public async Task InteriorChecker()
        {
            await Delay(1000);
            if (GetInteriorFromEntity(Game.PlayerPed.Handle) == NIGHTCLUB_INTERIOR_ID)
            {
                if (IsInInterior != true)
                {
                    IsInInterior = true;
                    InteriorChanged(true);
                }
            }
            else
            {
                if (IsInInterior != false)
                {
                    IsInInterior = false;
                    InteriorChanged(false);
                }
            }
        }

        private List<string> emitters = new List<string>()
        {
            "SE_BA_DLC_INT_01_BOGS",
            "SE_BA_DLC_INT_01_ENTRY_HALL",
            "SE_BA_DLC_INT_01_ENTRY_STAIRS",
            "SE_BA_DLC_INT_01_GARAGE",
            "SE_BA_DLC_INT_01_MAIN_AREA_2",
            "SE_BA_DLC_INT_01_MAIN_AREA",
            "SE_BA_DLC_INT_01_OFFICE",
            "SE_BA_DLC_INT_01_REAR_L_CORRIDOR",
        };

        private List<string> djRadioStations = new List<string>()
        {
            "RADIO_22_DLC_BATTLE_MIX1_CLUB",
            "RADIO_23_DLC_BATTLE_MIX2_CLUB",
            "RADIO_24_DLC_BATTLE_MIX3_CLUB",
            "RADIO_25_DLC_BATTLE_MIX4_CLUB",
        };


        // ped models: s_f_y_clubbar_01 & s_m_y_doorman_01
        private List<int> interiorPeds = new List<int>();

        private void InteriorChanged(bool enteredInterior)
        {
            if (enteredInterior)
            {
                TriggerEvent("weapons:setWeaponsEnabled", "fists");

                foreach (var ped in interiorPeds)
                {
                    int p = ped;
                    if (DoesEntityExist(p))
                    {
                        DeleteEntity(ref p);
                    }
                }


                interiorPeds.Clear();

                //CreateModelHide(-1605.643f, -3012.672f, -77.79608f, 1f, (uint)GetHashKey("ba_prop_club_screens_02"), true);
                CreateModelHide(-1605.643f, -3012.672f, -77.79608f, 1f, (uint)GetHashKey("ba_prop_club_screens_01"), true);

                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_clubname_01");          // Name (galaxy)
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_Style01");              // Style (traditional)
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_style01_podium");       // Podium (traditional)
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_equipment_setup");      // Speaker setup (upgraded part 1)
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_equipment_upgrade");    // Speaker setup (upgraded part 2)
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_security_upgrade");     // Security (cameras and shit)
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_dj03");                 // DJ Booth (variant 3)
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "DJ_04_Lights_03");               // Ceiling neon lights (variant 4)
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_bar_content");          // Bar
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_booze_01");             // Booze 1
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_booze_02");             // Booze 2
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_booze_03");             // Booze 3
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_trophy03");             // Trophy variant 3
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_deliverytruck");        // Delivery truck in the garage
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_dry_ice");              // Dry ice machines (no smoke effects)
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_lightgrid_01");         // Roof lights
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_trad_lights");          // Floor/wall lights
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_trophy04");             // Chest in VIP lounge
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_trophy04");             // Chest in VIP lounge
                EnableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_dj01");                 // Dixon DJ posters
                DisableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_dj02");                // Remove other posters
                DisableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_dj03");                // Remove other posters
                DisableInteriorProp(NIGHTCLUB_INTERIOR_ID, "Int01_ba_dj04");                // Remove other posters

                if (!IsAudioSceneActive("DLC_Ba_NightClub_Scene"))
                {
                    StartAudioScene("DLC_Ba_NightClub_Scene");
                }

                foreach (string s in emitters)
                {
                    SetEmitterRadioStation(s, djRadioStations[2]);
                }

                EffectsManager.AddSmokeParticles();

                RefreshInterior(NIGHTCLUB_INTERIOR_ID);

                StartTv();

                CreatePeds();
            }
            else
            {
                //CitizenFX.Core.Native.Function.Call(CitizenFX.Core.Native.Hash.REMOVE_MODEL_HIDE, -1605.643f, -3012.672f, -77.79608f, 1f, (uint)GetHashKey("ba_prop_club_screens_02"), false);

                if (IsAudioSceneActive("DLC_Ba_NightClub_Scene"))
                {
                    StopAudioScene("DLC_Ba_NightClub_Scene");
                }

                EffectsManager.RemoveSmokeParticles();

                foreach (var ped in interiorPeds)
                {
                    int p = ped;
                    if (DoesEntityExist(p))
                    {
                        DeleteEntity(ref p);
                    }
                }
                interiorPeds.Clear();
                TriggerEvent("weapons:setWeaponsEnabled", "all");
            }
        }


        private readonly List<string> djVideos = new List<string>() { "SOL", "TOU", "DIX", "TBM" }; // Solomun, Tale of Us, Dixon, The Black Madonna
        private readonly List<string> screenTypes = new List<string>() { "LSER", "LED", "GEO", "RIB", "NL" }; // lasers, led tubes, neon tubes, ribbon bands, no light rig
        private readonly List<string> clubs = new List<string>() { "GALAXY", "LOS", "OMEGA", "TECH", "GEFANGNIS", "MAIS", "FUNHOUSE", "PALACE", "PARADISE" };

        public async void StartTv()
        {
            await Delay(2000);
            string renderTarget = "club_projector";
            int channel = 2;
            string dj = djVideos[2]; // dixon
            string screenType = screenTypes[3]; // neon tubes
            string clubName = clubs[0]; // galaxy

            int renderTargetHandle = 0;

            SetTvChannel(-1); // disable tv.

            async Task DoSetup()
            {
                LoadTvChannelSequence(channel, $"PL_{dj}_{screenType}_{clubName}", false);
                await Delay(500);

                ReleaseNamedRendertarget(renderTarget);

                await Delay(500);

                if (!IsNamedRendertargetRegistered(renderTarget))
                {
                    RegisterNamedRendertarget(renderTarget, false);
                }

                while (!IsNamedRendertargetRegistered(renderTarget))
                {
                    await Delay(0);
                }


                if (!IsNamedRendertargetLinked((uint)GetHashKey("ba_prop_club_screens_01")))
                {
                    LinkNamedRendertarget((uint)GetHashKey("ba_prop_club_screens_01"));
                }

                while (!IsNamedRendertargetLinked((uint)GetHashKey("ba_prop_club_screens_01")))
                {
                    await Delay(0);
                }


                renderTargetHandle = GetNamedRendertargetRenderId(renderTarget);


                SetTvChannel(channel);
                SetTvVolume(100f);
                SetTvAudioFrontend(false);
            }

            await DoSetup();

            int timer = GetGameTimer() + 1000;
            while (IsInInterior)
            {
                DisablePlayerFiring(PlayerId(), true);

                if (GetGameTimer() > timer)
                {
                    timer = GetGameTimer() + 1000;
                    SetTvChannel(channel);
                }
                await Delay(0);

                SetTextRenderId(renderTargetHandle);
                SetScriptGfxDrawOrder(4);
                SetScriptGfxDrawBehindPausemenu(true);

                DrawTvChannel(0.5f, 0.5f, 1f, 1f, 0f, 255, 255, 255, 255);

                SetScriptGfxDrawBehindPausemenu(false);
                SetTextRenderId(GetDefaultScriptRendertargetRenderId());
            }

            SetTvChannel(-1); // disable tv.
            ReleaseNamedRendertarget(renderTarget);
        }

        private async void CreatePeds()
        {
            uint bartenderModel = (uint)GetHashKey("s_f_y_clubbar_01");
            uint securityModel = (uint)GetHashKey("s_m_y_doorman_01");
            uint dancerModel1 = (uint)GetHashKey("u_f_y_dancerave_01");
            uint dancerModel2 = (uint)GetHashKey("u_m_y_dancerave_01");
            RequestModel(bartenderModel);
            RequestModel(securityModel);
            RequestModel(dancerModel1);
            RequestModel(dancerModel2);
            while (!HasModelLoaded(bartenderModel) || !HasModelLoaded(securityModel) || !HasModelLoaded(dancerModel1) || !HasModelLoaded(dancerModel2))
            {
                await Delay(0);
            }
            float offset = 1.0f;
            int bartender1 = CreatePed(0, bartenderModel, -1584.845f, -3012.627f, -76.006f - offset, 87.9f, false, false);
            int bartender2 = CreatePed(0, bartenderModel, -1577.435f, -3016.991f, -79.01f - offset, 7.2f, false, false);
            int bartender3 = CreatePed(0, bartenderModel, -1572.232f, -3013.227f, -74.407f - offset, 270.1f, false, false);
            int security1 = CreatePed(0, securityModel, -1604.581f, -3004.979f, -76.006f - offset, 184.5f, false, false);
            int security2 = CreatePed(0, securityModel, -1588.02f, -3006.703f, -76.006f - offset, 119.8f, false, false);
            int security3 = CreatePed(0, securityModel, -1599.824f, -3019.082f, -79.007f - offset, 349.0f, false, false);
            int security4 = CreatePed(0, securityModel, -1612.709f, -3006.101f, -79.007f - offset, 268.8f, false, false);
            int security5 = CreatePed(0, securityModel, -1575.375f, -3007.959f, -79.006f - offset, 87.7f, false, false);
            int security6 = CreatePed(0, securityModel, -1568.521f, -3003.071f, -76.207f - offset, 136.2f, false, false);
            int dancer1 = CreatePed(0, dancerModel1, -1594.51f, -3014.158f, -79.006f - offset, 49.4f, false, false); // female
            int dancer2 = CreatePed(0, dancerModel2, -1598.061f, -3009.232f, -79.0f - offset, 222.9f, false, false); // male
            SetPedDefaultComponentVariation(dancer1);
            SetPedDefaultComponentVariation(dancer2);


            SetModelAsNoLongerNeeded(bartenderModel);
            SetModelAsNoLongerNeeded(securityModel);
            SetModelAsNoLongerNeeded(dancerModel1);
            SetModelAsNoLongerNeeded(dancerModel2);

            TaskStartScenarioInPlace(bartender1, "WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT", 0, true);
            TaskStartScenarioInPlace(bartender2, "WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT", 0, true);
            TaskStartScenarioInPlace(bartender3, "WORLD_HUMAN_STAND_IMPATIENT_UPRIGHT", 0, true);
            TaskStartScenarioInPlace(security1, "WORLD_HUMAN_GUARD_STAND", 0, true);
            TaskStartScenarioInPlace(security2, "WORLD_HUMAN_GUARD_STAND", 0, true);
            TaskStartScenarioInPlace(security3, "WORLD_HUMAN_GUARD_STAND", 0, true);
            TaskStartScenarioInPlace(security4, "WORLD_HUMAN_GUARD_STAND", 0, true);
            TaskStartScenarioInPlace(security5, "WORLD_HUMAN_GUARD_STAND", 0, true);
            TaskStartScenarioInPlace(security6, "WORLD_HUMAN_GUARD_STAND", 0, true);
            var animDict = "anim@amb@nightclub@dancers@podium_dancers@";
            //var animDict = "anim@amb@nightclub@dancers@crowddance_groups@med_intensity";
            RequestAnimDict(animDict);
            while (!HasAnimDictLoaded(animDict))
            {
                await Delay(0);
            }
            TaskPlayAnim(dancer1, animDict, "hi_dance_facedj_17_v2_female^2", 8.0f, 1.0f, -1, 1, 0, false, false, false);
            TaskPlayAnim(dancer2, animDict, "hi_dance_facedj_17_v2_male^5", 8.0f, 1.0f, -1, 1, 0, false, false, false);
            RemoveAnimDict(animDict);

            interiorPeds.Add(bartender1);
            interiorPeds.Add(bartender2);
            interiorPeds.Add(bartender3);
            interiorPeds.Add(security1);
            interiorPeds.Add(security2);
            interiorPeds.Add(security3);
            interiorPeds.Add(security4);
            interiorPeds.Add(security5);
            interiorPeds.Add(security6);
            interiorPeds.Add(dancer1);
            interiorPeds.Add(dancer2);

            foreach (var ped in interiorPeds)
            {
                ForceRoomForEntity(ped, NIGHTCLUB_INTERIOR_ID, (uint)GetRoomKeyFromEntity(ped));
                FreezeEntityPosition(ped, true);
                SetBlockingOfNonTemporaryEvents(ped, true);
                SetPedKeepTask(ped, true);
                SetEntityInvincible(ped, true);
                StopPedSpeaking(ped, true);
                DisablePedPainAudio(ped, true);
                SetPedConfigFlag(ped, 118, false);
                SetPedConfigFlag(ped, 208, true);
                SetPedRelationshipGroupHash(ped, (uint)GetHashKey("player"));
            }
        }
    }
}

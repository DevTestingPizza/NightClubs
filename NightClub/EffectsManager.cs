using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace NightClub
{
    class EffectsManager : BaseScript
    {

        public EffectsManager()
        {

        }

        internal int[] lampObjects { get; private set; } = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        internal int[] rotatorObjects { get; private set; } = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        internal int[] beamObjects { get; private set; } = new int[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        internal Vector3[] lampPositions = new Vector3[9]
        {
            new Vector3(-1591.597f, -3013.749f, -77.3800f),
            new Vector3(-1591.597f, -3010.702f, -77.3800f),
            new Vector3(-1594.379f, -3008.334f, -77.3800f),
            new Vector3(-1597.731f, -3008.334f, -77.3800f),
            new Vector3(-1602.168f, -3010.862f, -76.9113f),
            new Vector3(-1602.168f, -3014.519f, -76.9113f),
            new Vector3(-1606.630f, -3010.431f, -75.7108f),
            new Vector3(-1606.629f, -3014.947f, -75.7108f),
            new Vector3(-1602.368f, -3018.949f, -77.3800f),
        };
        internal Vector3[] rotatorPositions = new Vector3[9]
        {
            new Vector3(-1591.597f, -3013.749f, -77.1822f),
            new Vector3(-1591.597f, -3010.702f, -77.1822f),
            new Vector3(-1594.379f, -3008.334f, -77.1822f),
            new Vector3(-1597.731f, -3008.334f, -77.1822f),
            new Vector3(-1602.168f, -3010.862f, -76.7135f),
            new Vector3(-1602.168f, -3014.519f, -76.7135f),
            new Vector3(-1606.63f, -3010.431f, -75.513f),
            new Vector3(-1606.629f, -3014.947f, -75.513f),
            new Vector3(-1602.368f, -3018.949f, -77.1822f),
        };

        internal Vector3[] lampRotations = new Vector3[9]
        {
            new Vector3(0f, 0f, 180f),
            new Vector3(0f, 0f, 180f),
            new Vector3(0f, 0f, -90f),
            new Vector3(0f, 0f, -90f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, -30f),
            new Vector3(0f, 0f, 30f),
            new Vector3(0f, 0f, 40f),
        };

        internal List<List<Vector3>> roofLightsCoords = new List<List<Vector3>>()
        {
            new List<Vector3>()
            {
                new Vector3(3.7219f, 3.4921f, 5.721f),
                new Vector3(3.7219f, 2.1514f, 5.4099f),
                new Vector3(3.7219f, 0.7985f, 5.721f),
                new Vector3(3.7219f, -1.0028f, 5.721f),
                new Vector3(3.7219f, -2.3244f, 5.4099f),
                new Vector3(3.7219f, -3.7085f, 5.721f)
            },
            new List<Vector3>()
            {
                new Vector3(5.8927f, 3.4921f, 5.721f),
                new Vector3(5.8927f, 2.1514f, 5.4099f),
                new Vector3(5.8927f, 0.7985f, 5.721f),
                new Vector3(5.8927f, -1.0028f, 5.721f),
                new Vector3(5.8927f, -2.3244f, 5.4099f),
                new Vector3(5.8927f, -3.7085f, 5.721f)
            },
            new List<Vector3>()
            {
                new Vector3(8.0635f, 3.4921f, 5.721f),
                new Vector3(8.0635f, 2.1514f, 5.4099f),
                new Vector3(8.0635f, 0.7985f, 5.721f),
                new Vector3(8.0635f, -1.0028f, 5.721f),
                new Vector3(8.0635f, -2.3244f, 5.4099f),
                new Vector3(8.0635f, -3.7085f, 5.721f)
            },
            new List<Vector3>()
            {
                new Vector3(10.2343f, 3.4921f, 5.721f),
                new Vector3(10.2343f, 2.1514f, 5.4099f),
                new Vector3(10.2343f, 0.7985f, 5.721f),
                new Vector3(10.2343f, -1.0028f, 5.721f),
                new Vector3(10.2343f, -2.3244f, 5.4099f),
                new Vector3(10.2343f, -3.7085f, 5.721f)
            },
            new List<Vector3>()
            {
                new Vector3(12.4051f, 3.4921f, 5.721f),
                new Vector3(12.4051f, 2.1514f, 5.4099f),
                new Vector3(12.4051f, 0.7985f, 5.721f),
                new Vector3(12.4051f, -1.0028f, 5.721f),
                new Vector3(12.4051f, -2.3244f, 5.4099f),
                new Vector3(12.4051f, -3.7085f, 5.721f)
            }
        };

        internal async void CreateLight(int club, int firstRandom, int secondRandom)
        {
            var modellamp = (uint)GetHashKey("ba_prop_club_emis_rig_10_shad");
            RequestModel(modellamp);
            while (!HasModelLoaded(modellamp))
            {
                await Delay(0);
            }

            var c = roofLightsCoords[firstRandom][secondRandom];

            var pos = GetObjectOffsetFromCoords(c.X, c.Y, c.Z, 0f, IplManager.interiorLocation.X, IplManager.interiorLocation.Y, IplManager.interiorLocation.Z - 2f);
            if (DoesObjectOfTypeExistAtCoords(pos.X, pos.Y, pos.Z, 0.1f, modellamp, false))
            {
                var tmp = GetClosestObjectOfType(pos.X, pos.Y, pos.Z, 0.1f, modellamp, false, false, false);
                DeleteObject(ref tmp);
            }

            var objs = CreateObjectNoOffset(modellamp, pos.X, pos.Y, pos.Z, false, false, false);

            var color = GetRandomLightColor();
            SetBeamColor(objs, color);

            for (var i = 0; i < (255 / 4); i++)
            {
                if (i * 4 > 250)
                {
                    ResetEntityAlpha(objs);
                }
                else
                {
                    SetEntityAlpha(objs, i * 4, 0);
                }
                await Delay(0);
            }
            for (var i = (255 / 4); i > 0; i--)
            {
                if (i * 4 > 250)
                {
                    ResetEntityAlpha(objs);
                }
                else
                {
                    SetEntityAlpha(objs, i * 4, 0);
                }
                await Delay(0);
            }
            //await Delay(3000);
            DeleteObject(ref objs);
        }

        private static Random randomizer = new Random();

        private List<int[]> lightColors = new List<int[]>()
        {
                       //  r    g    b    a
            new int[4] { 255,   7, 255, 160 }, // 0 - pink/purple
            new int[4] { 255, 255, 145,   0 }, // 1 - soft yellow
            new int[4] { 145, 145, 253, 255 }, // 2 - light violet/purple/white/blue

            new int[4] { 255, 226, 57, 160 }, // 3 - bright yellow
            new int[4] { 50, 200, 255,   0 }, // 4 - bright blue
            new int[4] { 255, 255, 255,   0 }, // 4 - white
            //new int[4] { 20, 255, 20,   0 }, // 4 - green
            //new int[4] { 145, 145, 253, 255 }, // 5
            //new int[4] { 255,   7, 255, 160 }, // 3
            //new int[4] { 255, 255, 145,   0 }, // 4
            //new int[4] { 145, 145, 253, 255 }, // 5
            //new int[4] { 255,   7, 255, 160 }, // 6
            //new int[4] { 255, 255, 145,   0 }, // 7
            //new int[4] { 145, 145, 253, 255 }, // 8
        };

        internal int[] GetRandomLightColor()
        {
            return lightColors[randomizer.Next(0, lightColors.Count)];
        }

        private void SetBeamColor(int beam, int[] color)
        {
            CitizenFX.Core.Native.Function.Call((CitizenFX.Core.Native.Hash)0xDF7B44882EE79164, beam, 1, color[0], color[1], color[2]);
        }

        internal async void EnableLights(int interior)
        {
            while (interior == 0 || !IsInteriorReady(interior))
            {
                await Delay(0);
            }
            uint lampModel = (uint)GetHashKey("ba_prop_battle_lights_fx_lamp");
            uint rotatorModel = (uint)GetHashKey("ba_prop_battle_lights_fx_rotator");
            string lightBeamName = "ba_prop_battle_lights_fx_rig";

            for (var i = 0; i < 9; i++)
            {
                var lamp = lampObjects[i];
                var rotator = rotatorObjects[i];
                var beam = beamObjects[i];
                Vector3 lampPos = lampPositions[i];
                Vector3 rotatorPos = rotatorPositions[i];
                Vector3 beamPos = rotatorPositions[i];
                uint beamModel = (uint)GetHashKey(lightBeamName + (i % 2 == 0 ? "f" : "d"));

                if (DoesEntityExist(beam))
                {
                    DeleteObject(ref beam);
                    beamObjects[i] = 0;
                }
                if (DoesEntityExist(rotator))
                {
                    DeleteObject(ref rotator);
                    rotatorObjects[i] = 0;
                }
                if (DoesEntityExist(lamp))
                {
                    DeleteObject(ref lamp);
                    lampObjects[i] = 0;
                }

                lamp = GetClosestObjectOfType(lampPos.X, lampPos.Y, lampPos.Z, 3f, lampModel, false, false, false);
                rotator = GetClosestObjectOfType(rotatorPos.X, rotatorPos.Y, rotatorPos.Z, 3f, rotatorModel, false, false, false);
                beam = GetClosestObjectOfType(beamPos.X, beamPos.Y, beamPos.Z, 3f, beamModel, false, false, false);

                if (DoesEntityExist(beam))
                {
                    DeleteObject(ref beam);
                }
                if (DoesEntityExist(rotator))
                {
                    DeleteObject(ref rotator);
                }
                if (DoesEntityExist(lamp))
                {
                    DeleteObject(ref lamp);
                }

                beam = 0;
                rotator = 0;
                lamp = 0;

                CitizenFX.Core.Native.Function.Call(CitizenFX.Core.Native.Hash.REMOVE_MODEL_HIDE, lampPos.X, lampPos.Y, lampPos.Z, 3f, lampModel, false);
                CitizenFX.Core.Native.Function.Call(CitizenFX.Core.Native.Hash.REMOVE_MODEL_HIDE, rotatorPos.X, rotatorPos.Y, rotatorPos.Z, 3f, rotatorModel, false);

                lamp = CreateObjectNoOffset(lampModel, lampPos.X, lampPos.Y, lampPos.Z, false, false, false);
                rotator = CreateObjectNoOffset(rotatorModel, rotatorPos.X, rotatorPos.Y, rotatorPos.Z, false, false, false);
                beam = CreateObjectNoOffset(beamModel, beamPos.X, beamPos.Y, beamPos.Z, false, false, true);

                ForceRoomForEntity(lamp, interior, 2937632879);
                ForceRoomForEntity(rotator, interior, 2937632879);
                ForceRoomForEntity(beam, interior, 2937632879);

                SetEntityRotation(lamp, lampRotations[i].X, lampRotations[i].Y, lampRotations[i].Z, 2, true);
                SetEntityRotation(rotator, lampRotations[i].X, lampRotations[i].Y, lampRotations[i].Z, 2, true);

                //AttachEntityToEntity(rotator, lamp, -1, 0f, 0f, 0.21f, 0f, 0f, 0f, false, false, false, false, 2, true);
                AttachEntityToEntity(beam, lamp, -1, 0f, 0f, 0.21f, 0f, 0f, 0f, false, false, false, false, 2, true);

                CreateModelHideExcludingScriptObjects(lampPos.X, lampPos.Y, lampPos.Z, 3f, lampModel, true);
                CreateModelHideExcludingScriptObjects(rotatorPos.X, rotatorPos.Y, rotatorPos.Z, 3f, rotatorModel, true);

                ResetEntityAlpha(beam);

                var color = GetRandomLightColor();
                SetBeamColor(beam, color);

                lampObjects[i] = lamp;
                rotatorObjects[i] = rotator;
                beamObjects[i] = beam;

                // ba_prop_club_emis_rig_10_shad            AC028888   -1409120120   2885847176
            }
        }



        bool reverse = false;
        //int timer = GetGameTimer();
        //int randomer = new Random().Next(100, 1000);
        int newLightTimer1 = GetGameTimer();
        int newLightTimer2 = GetGameTimer() + 2000;
        int newLightTimer3 = GetGameTimer() + 4000;
        int newLightTimer4 = GetGameTimer() + 6000;


        string RandomBeam => new string[8] { "ba_prop_battle_lights_fx_riga", "ba_prop_battle_lights_fx_rigb", "ba_prop_battle_lights_fx_rigc", "ba_prop_battle_lights_fx_rigd", "ba_prop_battle_lights_fx_rige", "ba_prop_battle_lights_fx_rigf", "ba_prop_battle_lights_fx_rigg", "ba_prop_battle_lights_fx_righ" }[randomizer.Next(0, 8)];

        int step = 0;

        private async Task RefreshLights(int club)
        {
            foreach (var tmpBeam in beamObjects)
            {
                SetEntityAlpha(tmpBeam, 0, 0);
            }

            await Delay(500);

            var iter = 0;
            foreach (var lamp in lampObjects)
            {
                SetEntityRotation(lamp, lampRotations[iter].X, lampRotations[iter].Y, lampRotations[iter].Z, 2, true);
                SetEntityRotation(rotatorObjects[iter], lampRotations[iter].X, lampRotations[iter].Y, lampRotations[iter].Z, 2, true);
                iter++;
            }

            await Delay(500);

            for (var bi = 0; bi < beamObjects.Length; bi++)
            {
                var tmpbeam = beamObjects[bi];
                DeleteEntity(ref tmpbeam);
                var newBeam = CreateObjectNoOffset((uint)GetHashKey(RandomBeam), rotatorPositions[bi].X, rotatorPositions[bi].Y, rotatorPositions[bi].Z, false, false, true);
                ForceRoomForEntity(newBeam, club, 2937632879);

                AttachEntityToEntity(newBeam, lampObjects[bi], -1, 0f, 0f, 0.21f, 0f, 0f, 0f, false, false, false, false, 2, true);
                beamObjects[bi] = newBeam;
            }
            foreach (var tmpBeam in beamObjects)
            {
                ResetEntityAlpha(tmpBeam);
                var color = GetRandomLightColor();
                SetBeamColor(tmpBeam, color);
            }
        }

        private float rotationAngle = 0f;

        [Tick]
        private async Task NightClubLightAnimations()
        {
            if (IplManager.IsInInterior)
            {
                if ((!DoesEntityExist(beamObjects[0]) || !IsInteriorReady(IplManager.NIGHTCLUB_INTERIOR_ID)))
                {
                    EnableLights(IplManager.NIGHTCLUB_INTERIOR_ID);
                }
                else
                {
                    if (GetGameTimer() - newLightTimer1 > 800)
                    {
                        newLightTimer1 = GetGameTimer();
                        CreateLight(IplManager.NIGHTCLUB_INTERIOR_ID, randomizer.Next(0, 5), randomizer.Next(0, 6));
                    }
                    if (GetGameTimer() - newLightTimer2 > 900)
                    {
                        newLightTimer2 = GetGameTimer();
                        CreateLight(IplManager.NIGHTCLUB_INTERIOR_ID, randomizer.Next(0, 5), randomizer.Next(0, 6));
                    }
                    if (GetGameTimer() - newLightTimer3 > 1000)
                    {
                        newLightTimer3 = GetGameTimer();
                        CreateLight(IplManager.NIGHTCLUB_INTERIOR_ID, randomizer.Next(0, 5), randomizer.Next(0, 6));
                    }
                    if (GetGameTimer() - newLightTimer4 > 1100)
                    {
                        newLightTimer4 = GetGameTimer();
                        CreateLight(IplManager.NIGHTCLUB_INTERIOR_ID, randomizer.Next(0, 5), randomizer.Next(0, 6));
                    }


                    //var ii = 0;
                    var index = 0;

                    foreach (var obj in lampObjects)
                    {

                        var beam = beamObjects[index];

                        var rot = GetEntityRotation(obj, 2);
                        if (DoesEntityExist(obj))
                        {
                            if (step < 4)
                            {
                                if (index == 1)
                                {
                                    if (rotationAngle > 4.0f)
                                    {
                                        foreach (var l in beamObjects)
                                        {
                                            SetEntityAlpha(l, 0, 0);
                                        }
                                        await Delay(1000);
                                        for (int l = 0; l < 9; l++)
                                        {
                                            SetEntityRotation(lampObjects[l], lampRotations[l].X, lampRotations[l].Y, lampRotations[l].Z, 2, true);
                                            SetEntityRotation(rotatorObjects[l], lampRotations[l].X, lampRotations[l].Y, lampRotations[l].Z, 2, true);
                                            var color = GetRandomLightColor();
                                            SetBeamColor(beamObjects[l], color);
                                        }
                                        foreach (var l in beamObjects)
                                        {
                                            ResetEntityAlpha(l);
                                        }
                                        step++;
                                        //reverse = !reverse;
                                        rotationAngle = 0f;
                                        break;
                                        //Debug.WriteLine("step up, color should change");
                                    }
                                    rotationAngle += 0.01f;
                                }

                                var rot2 = GetEntityRotation(rotatorObjects[index], 2);
                                float rotation = (float)((Math.PI * 2f) + (rotationAngle * 2f));

                                float radius = 0.8f;

                                var newRot1 = new Vector3(rot.X, (float)(radius * Math.Cos(rotation)) + rot.Y, (float)(radius * Math.Sin(rotation)) + rot.Z);
                                var newRot2 = new Vector3(rot.X, rot2.Y, (float)(radius * Math.Sin(rotation)) + rot2.Z);

                                SetEntityRotation(obj, newRot1.X, newRot1.Y, newRot1.Z, 2, true);
                                SetEntityRotation(rotatorObjects[index], newRot2.X, newRot2.Y, newRot2.Z, 2, true);

                            }
                            else if (step < 5)
                            {
                                await RefreshLights(IplManager.NIGHTCLUB_INTERIOR_ID);
                                step++;
                                break;
                            }
                            else if (step < 9)
                            {
                                if (index == 1)
                                {
                                    if (rot.Y < -20)
                                    {
                                        reverse = false;
                                    }
                                    else if (rot.Y > 70)
                                    {
                                        reverse = true;

                                        foreach (var c in beamObjects)
                                        {
                                            var color = GetRandomLightColor();
                                            SetBeamColor(c, color);
                                        }
                                        step++;
                                    }
                                }

                                //var lr = step > 6 ? 0.3f : -0.3f;
                                var lr = step % 2 == 0 ? (reverse ? 0.3f : -0.3f) : (reverse ? -0.3f : 0.3f);
                                var rot2 = GetEntityRotation(rotatorObjects[index], 2);
                                if (reverse)
                                {
                                    SetEntityRotation(obj, rot.X, rot.Y - 1f, rot.Z + lr, 2, true);
                                    SetEntityRotation(rotatorObjects[index], rot2.X, rot2.Y, rot2.Z + lr, 2, true);
                                }
                                else
                                {
                                    SetEntityRotation(obj, rot.X, rot.Y + 1f, rot.Z - lr, 2, true);
                                    SetEntityRotation(rotatorObjects[index], rot2.X, rot2.Y, rot2.Z - lr, 2, true);
                                }
                            }
                            else if (step < 10)
                            {
                                await RefreshLights(IplManager.NIGHTCLUB_INTERIOR_ID);
                                step++;
                                break;
                            }
                            else if (step < 14)
                            {
                                if (index == 1)
                                {
                                    if (rot.Y < -100f)
                                    {
                                        await Delay(100);
                                        reverse = false;
                                    }
                                    else if (rot.Y > 70f)
                                    {
                                        await Delay(100);
                                        reverse = true;
                                        foreach (var c in beamObjects)
                                        {
                                            var color = GetRandomLightColor();
                                            SetBeamColor(c, color);
                                        }
                                        step++;
                                    }
                                }

                                if (reverse)
                                {
                                    SetEntityRotation(obj, rot.X, rot.Y - 4f, rot.Z, 2, true);
                                }
                                else
                                {
                                    SetEntityRotation(obj, rot.X, rot.Y + 4f, rot.Z, 2, true);
                                }
                            }
                            else if (step >= 14)
                            {
                                rotationAngle = 0f;
                                step = 0;
                                //foreach (var tmpBeam in IplManager.beamObjects)
                                //{
                                //    SetEntityAlpha(tmpBeam, 0, 0);
                                //}

                                //await Delay(500);

                                //var iter = 0;
                                //foreach (var lamp in IplManager.lampObjects)
                                //{
                                //    SetEntityRotation(lamp, IplManager.lampRotations[iter].X, IplManager.lampRotations[iter].Y, IplManager.lampRotations[iter].Z, 2, true);
                                //    SetEntityRotation(IplManager.rotatorObjects[iter], IplManager.lampRotations[iter].X, IplManager.lampRotations[iter].Y, IplManager.lampRotations[iter].Z, 2, true);
                                //    iter++;
                                //}

                                //await Delay(500);

                                //foreach (var tmpBeam in IplManager.beamObjects)
                                //{
                                //    SetEntityAlpha(tmpBeam, 255, 0);
                                //}
                                await RefreshLights(IplManager.NIGHTCLUB_INTERIOR_ID);
                                break;
                            }
                        }
                        index++;
                    }
                }
            }
        }


        private static List<int> dryIceParticles = new List<int>() { 0, 0, 0, 0, 0, 0, 0 };
        private static readonly List<Vector3> dryIcePositions = new List<Vector3>()
        {
            // ambient
            new Vector3(-1600f, -3012f, -80f),
            new Vector3(-1595f, -3012f, -80f),
            new Vector3(-1590f, -3012f, -80f),

            // machines
            new Vector3(-1602.932f, -3019.1f, -79.99f),
            new Vector3(-1593.238f, -3017.05f, -79.99f),
            new Vector3(-1597.134f, -3008.2f, -79.99f),
            new Vector3(-1589.966f, -3008.518f, -79.99f)
        };
        private static readonly List<Vector3> dryIceRotations = new List<Vector3>()
        {
            // ambient
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 0f),
            new Vector3(0f, 0f, 0f),

            // machines
            new Vector3(0f, -10f, 66f),
            new Vector3(0f, -10f, 110f),
            new Vector3(0f, -10f, -122.53f),
            new Vector3(0f, -10f, -166.97f),
        };

        public static async void AddSmokeParticles()
        {
            RemoveSmokeParticles();

            if (!HasNamedPtfxAssetLoaded("scr_ba_club"))
            {
                RequestNamedPtfxAsset("scr_ba_club");
            }
            while (!HasNamedPtfxAssetLoaded("scr_ba_club"))
            {
                await Delay(0);
            }

            for (var i = 0; i < 7; i++)
            {
                int handle = dryIceParticles[i];
                Vector3 pos = dryIcePositions[i];
                Vector3 rotation = dryIceRotations[i];
                string particleName = i < 3 ? "scr_ba_club_smoke" : "scr_ba_club_smoke_machine";
                float scale = i < 3 ? 1f : 5f;
                if (!DoesParticleFxLoopedExist(handle))
                {
                    UseParticleFxAssetNextCall("scr_ba_club");
                    var z = pos.Z + (i < 3 ? 1f : 0f);
                    dryIceParticles[i] = StartParticleFxLoopedAtCoord(particleName, pos.X, pos.Y, z, rotation.X, rotation.Y, rotation.Z, scale, false, false, false, true);
                    SetParticleFxLoopedColour(dryIceParticles[i], 0.11f, 0.1f, 0.11f, true);
                }
                // Prop_Screen_Nightclub
            }
        }


        public static void RemoveSmokeParticles()
        {
            for (var i = 0; i < 7; i++)
            {
                int handle = dryIceParticles[i];
                if (DoesParticleFxLoopedExist(handle))
                {
                    RemoveParticleFx(handle, true);
                    dryIceParticles[i] = 0;
                }
            }
            if (HasNamedPtfxAssetLoaded("scr_ba_club"))
            {
                RemoveNamedPtfxAsset("scr_ba_club");
            }
        }

    }
}

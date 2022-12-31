//Don't mind all the errors you're seeing. The code works perfectly.
using Microsoft.Xna.Framework;
using ReLogic.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria;
using Guide.Content.Buffs;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using MrPlagueRaces.Common.Races;
using static Terraria.ModLoader.ModContent;

namespace Guide.Common.Races.Phoenix
{
    public class Phoenix : Race
    {
        public override void Load()
        {
            //These are for the text boxes on where your race is chosen.
            Description = "[c/512888:Mythical Races]\n[c/EB6123:Phoenix]\nCreated by the ashes.  Sent by the heavens\n[c/FFCC33:Lord Zereph's] [c/FFCC33:Mods]";
            AbilitiesDescription = "[c/6699CC:Passives]\n[c/FFCC33:Rebirth]\nRebirthing from a death has a 30 second cooldown, triggering 'Low Flames', but making you able to not die.\n[c/FFCC33:Active Flames]\nWhile playing, you have a permanently active furnace.\n[c/FFCC33:Wings]\nAble to fly.";
            //What style of clothing/hair your race will have when selected
            ClothStyle = 5;
            HairStyle = 15;
            //Basically the stuff that blocks your race from showing anything lewd.
            CensorClothing = false;
            //The clothes that you begin with when making a character.
            StarterShirt = false;
            StarterPants = false;
            //What the default colors will be when selecting your race.
            //(Red, Green, Blue).
            HairColor = new Color(255, 255, 255);
            SkinColor = new Color(255, 255, 255);
            EyeColor = new Color(255, 255, 255);
            ShirtColor = new Color(255, 255, 255);
            UnderShirtColor = new Color(255, 255, 255);
            PantsColor = new Color(255, 255, 255);
        }

        public override void ResetEffects(Player player)
        {
            var modPlayer = player.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();
            if (modPlayer.statsEnabled)
            {
                //You put race stats here
                player.pickSpeed -= 0.5f;
                player.statLifeMax2 += (int)(player.statLifeMax2 / 2);
                player.lifeRegen += 5;
                player.statManaMax2 -= player.statManaMax2 - (int)(player.statManaMax2 / 1.2);
                player.endurance += 0.41f;
                player.GetDamage(DamageClass.Melee).Base += 0.175f;
                player.GetDamage(DamageClass.Ranged).Base -= 0.5f;
                player.GetDamage(DamageClass.Magic).Base += .0125f;
                player.GetDamage(DamageClass.Summon).Base -= 1.5f;
                player.GetCritChance(DamageClass.Ranged) -= 0.16f;
                player.moveSpeed += 0.5f;
                player.adjTile[17] = true;
                player.oldAdjTile[17] = true;
            }
        }

        public void Kill(Player player, double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
        {
            if (!player.HasBuff(BuffType<BurningLow>()))
            {
                player.dead = false;
                player.statLife += (int)(player.statLifeMax2 / 2);
                player.AddBuff(BuffType<BurningLow>(), 1800);
            }
        }

        public override void ProcessTriggers(Player player, TriggersSet triggersSet)
        {
            var mrPlagueRacesPlayer = player.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();
            var kenkuPlayer = player.GetModPlayer<PhoenixResPlayer>();
            float anchor = 200;
            if (player.statDefense > 30)
            {
                anchor = 480;
            }
            if (mrPlagueRacesPlayer.statsEnabled)
            {
                if (!player.dead)
                {
                    if (MrPlagueRaces.MrPlagueRaces.RaceAbilityKeybind1.JustPressed)
                    {

                    }
                    if (MrPlagueRaces.MrPlagueRaces.RaceAbilityKeybind3.JustPressed)
                    {

                    }
                    if (MrPlagueRaces.MrPlagueRaces.RaceAbilityKeybind2.JustPressed)
                    {

                    }
                    int maxVelocity = (6 + (player.statLifeMax2 / 20));
                    if (player.wings == 0)
                    {
                        if (player.controlJump)
                        {
                            kenkuPlayer.dashTime = 0;
                            player.fallStart = (int)(player.position.Y / 16f);
                            if (player.velocity.Y != 0)
                            {
                                kenkuPlayer.flying = true;
                            }
                            if (kenkuPlayer.wingTime > 0)
                            {
                                kenkuPlayer.wingFrameCounter++;
                                if (kenkuPlayer.wingFrameCounter > 4)
                                {
                                    kenkuPlayer.wingFrame++;
                                    kenkuPlayer.wingFrameCounter = 0;
                                    if (kenkuPlayer.wingFrame >= 4)
                                    {
                                        kenkuPlayer.wingFrame = 0;
                                        SoundEngine.PlaySound(SoundID.Item32, player.Center);
                                    }
                                }
                                float ascentWhenFalling = (float)(anchor / 100);
                                float ascentWhenRising = (float)(anchor / 420);
                                float maxCanAscendMultiplier = (float)(anchor / 100);
                                float maxAscentMultiplier = (float)(anchor / 25);
                                float constantAscend = (float)(anchor / 500);

                                player.velocity.Y -= ascentWhenFalling * player.gravDir;

                                if (player.gravDir == 1f)
                                {
                                    if (player.velocity.Y > 0f)
                                    {
                                        player.velocity.Y -= ascentWhenRising;
                                    }
                                    else if (player.velocity.Y > (0f - 5) * maxAscentMultiplier)
                                    {
                                        player.velocity.Y -= constantAscend;
                                    }
                                    if (player.velocity.Y < (0f - 5) * maxCanAscendMultiplier)
                                    {
                                        player.velocity.Y = (0f - 5) * maxCanAscendMultiplier;
                                    }
                                }
                                else
                                {
                                    if (player.velocity.Y < 0f)
                                    {
                                        player.velocity.Y += ascentWhenRising;
                                    }
                                    else if (player.velocity.Y < 5 * maxAscentMultiplier)
                                    {
                                        player.velocity.Y += constantAscend;
                                    }
                                    if (player.velocity.Y > 5 * maxCanAscendMultiplier)
                                    {
                                        player.velocity.Y = 5 * maxCanAscendMultiplier;
                                    }
                                }
                                kenkuPlayer.wingTime -= 1f;
                            }
                            else
                            {
                                if (player.velocity.Y > 3f)
                                {
                                    player.velocity.Y = 3f;
                                }
                                if (player.velocity.Y > 1)
                                {
                                    kenkuPlayer.wingFrameCounter = 0;
                                    kenkuPlayer.wingFrame = 2;
                                }
                            }
                            if (player.controlLeft && player.velocity.X > ((player.maxRunSpeed * 2) * -1))
                            {
                                player.velocity.X += -0.1f;
                            }
                            if (player.controlRight && player.velocity.X < (player.maxRunSpeed * 2))
                            {
                                player.velocity.X += 0.1f;
                            }
                        }
                        else if (kenkuPlayer.dashTime == 0)
                        {
                            kenkuPlayer.wingFrameCounter = 0;
                            kenkuPlayer.wingFrame = 0;
                        }
                        if (player.empressBrooch && kenkuPlayer.wingTime != 0f)
                        {
                            kenkuPlayer.wingTime = player.statLifeMax2;
                        }
                    }
                    if (player.velocity.Y == 0)
                    {
                        kenkuPlayer.wingTime = ((player.statLifeMax2 / 4) - (player.statDefense * 2));
                    }
                }
                if (!player.controlJump || player.velocity.Y == 0)
                {
                    kenkuPlayer.flying = false;
                }
            }
        }

        //This is so that your race rotates when sleeping.
        public override void ModifyDrawInfo(Player player, ref PlayerDrawSet drawInfo)
        {
            if (!player.sleeping.isSleeping)
            {
                player.fullRotation = 0f;
            }
        }

        public override void PreUpdate(Player player)
        {
            var mrPlagueRacesPlayer = player.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();
            var kenkuPlayer = player.GetModPlayer<PhoenixResPlayer>();
            if (mrPlagueRacesPlayer.statsEnabled)
            {
                if (player.velocity.Y == 0)
                {
                    kenkuPlayer.wingFrameCounter = 0;
                    kenkuPlayer.wingFrame = 0;
                }
                else if (player.velocity.Y != 0 && !player.controlJump && kenkuPlayer.dashTime == 0)
                {
                    kenkuPlayer.wingFrameCounter = 0;
                    kenkuPlayer.wingFrame = 1;
                }
                if (kenkuPlayer.dashTime > 0)
                {
                    kenkuPlayer.dashTime--;
                    kenkuPlayer.wingFrameCounter++;
                    if (kenkuPlayer.wingFrameCounter > 6)
                    {
                        kenkuPlayer.wingFrame++;
                        kenkuPlayer.wingFrameCounter = 0;
                        if (kenkuPlayer.wingFrame >= 3)
                        {
                            kenkuPlayer.wingFrame = 0;
                            SoundEngine.PlaySound(SoundID.Item32, player.Center);
                        }
                    }
                }
            }
        }
    }
    public class PhoenixResPlayer : ModPlayer
    {
        public float wingTime;
        public bool flying;
        public int wingFrame;
        public int wingFrameCounter;
        public int dashTime;

    }

    public class PhoenixWings : PlayerDrawLayer
    {
        private Asset<Texture2D>[] Wings_Texture = new Asset<Texture2D>[10];
        private string[] PlayerColors = { "ColorSkin", "ColorDetail", "Colorless", "ColorEyes", "ColorHair", "ColorSkin/Glowmask", "ColorDetail/Glowmask", "Colorless/Glowmask", "ColorEyes/Glowmask", "ColorHair/Glowmask" };

        public override Position GetDefaultPosition() => new BeforeParent(PlayerDrawLayers.Wings);

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            return (drawInfo.skinVar < 10 && drawPlayer.wings == 0);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            Vector2 helmetOffset = drawInfo.helmetOffset;

            var mrPlagueRacesPlayer = drawPlayer.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();
            var kenkuPlayer = drawPlayer.GetModPlayer<PhoenixResPlayer>();
            if (mrPlagueRacesPlayer.race != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    Wings_Texture[i] = mrPlagueRacesPlayer.GetRaceTexture(drawPlayer, $"{PlayerColors[i]}/Wings");
                }
                Vector2 bodyPosition = new Vector2((float)(int)(drawInfo.Position.X - Main.screenPosition.X - (float)(drawPlayer.bodyFrame.Width / 2) + (float)(drawPlayer.width / 2)), (float)(int)(drawInfo.Position.Y - Main.screenPosition.Y + (float)drawPlayer.height - (float)drawPlayer.bodyFrame.Height + 4f)) + drawPlayer.bodyPosition + new Vector2((float)(drawPlayer.bodyFrame.Width / 2), (float)(drawPlayer.bodyFrame.Height / 2));
                MakeColoredDrawDatas(ref drawInfo, Wings_Texture, null, new Vector2(bodyPosition.X - (9 * drawPlayer.direction), bodyPosition.Y - 9), new Rectangle(0, Wings_Texture[0].Height() / 4 * kenkuPlayer.wingFrame, Wings_Texture[0].Width(), Wings_Texture[0].Height() / 4), drawPlayer.bodyRotation, new Vector2((float)(Wings_Texture[0].Width() / 2), (float)(Wings_Texture[0].Height() / 14)), 1f, drawInfo.playerEffect, 0);
            }
        }
        private void MakeColoredDrawDatas(ref PlayerDrawSet drawInfo, Asset<Texture2D>[] texture, Asset<Texture2D>[,] textureHair, Vector2 position, Rectangle? sourceRect, float rotation, Vector2 origin, float scale, SpriteEffects effect, int inactiveLayerDepth)
        {
            DrawData drawData;
            Player drawPlayer = drawInfo.drawPlayer;
            int index;
            for (index = 0; index < 10; index++)
            {
                if (textureHair != null && textureHair[index, drawPlayer.hair] != ModContent.Request<Texture2D>("Guide/Assets/Textures/Blank"))
                {
                    drawData = new DrawData(textureHair[index, drawPlayer.hair].Value, position, sourceRect, PlayerColor(ref drawInfo, index), rotation, origin, scale, effect, 0);
                    drawData.shader = PlayerShader(ref drawInfo, index);
                    drawInfo.DrawDataCache.Add(drawData);
                }
                if (texture != null && texture[index] != ModContent.Request<Texture2D>("Guide/Assets/Textures/Blank"))
                {
                    drawData = new DrawData(texture[index].Value, position, sourceRect, PlayerColor(ref drawInfo, index), rotation, origin, scale, effect, 0);
                    drawData.shader = PlayerShader(ref drawInfo, index);
                    drawInfo.DrawDataCache.Add(drawData);
                }
            }
        }
        private Color PlayerColor(ref PlayerDrawSet drawInfo, int index)
        {
            Player drawPlayer = drawInfo.drawPlayer;
            var mrPlagueRacesPlayer = drawPlayer.GetModPlayer<MrPlagueRaces.MrPlagueRacesPlayer>();
            Color color = (index == 0 ? drawInfo.colorHead : index == 1 ? mrPlagueRacesPlayer.colorDetail : index == 2 ? drawInfo.colorEyeWhites : index == 3 ? drawInfo.colorEyes : index == 4 ? drawInfo.colorHair : index == 5 ? drawPlayer.GetImmuneAlpha(drawPlayer.skinColor, 0f) : index == 6 ? drawPlayer.GetImmuneAlpha(mrPlagueRacesPlayer.detailColor, 0f) : index == 7 ? drawPlayer.GetImmuneAlpha(Color.White, 0f) : index == 8 ? drawPlayer.GetImmuneAlpha(drawPlayer.eyeColor, 0f) : drawPlayer.GetImmuneAlpha(drawPlayer.GetHairColor(useLighting: false), 0f));
            return color;
        }

        private int PlayerShader(ref PlayerDrawSet drawInfo, int index)
        {
            int shader = (index == 0 ? drawInfo.skinDyePacked : index == 1 ? drawInfo.skinDyePacked : index == 2 ? 0 : index == 3 ? 0 : index == 4 ? drawInfo.hairDyePacked : index == 5 ? drawInfo.skinDyePacked : index == 6 ? drawInfo.skinDyePacked : index == 7 ? 0 : index == 8 ? 0 : drawInfo.hairDyePacked);
            return shader;
        }
    }
}
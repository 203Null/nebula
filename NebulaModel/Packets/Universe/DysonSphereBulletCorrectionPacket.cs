﻿using NebulaAPI;
using UnityEngine;

namespace NebulaModel.Packets.Universe
{
    public class DysonSphereBulletCorrectionPacket
    {
        public int StarIndex { get; set; }
        public int BulletId { get; set; }
        public Float3 UEndVel { get; set; }
        public Float3 UEnd { get; set; }

        public DysonSphereBulletCorrectionPacket() { }

        public DysonSphereBulletCorrectionPacket(int starIndex, int bulletId, Vector3 uEndVel, Vector3 uEnd)
        {
            StarIndex = starIndex;
            BulletId = bulletId;
            UEndVel = new Float3(uEndVel);
            UEnd = new Float3(uEnd);
        }
    }
}

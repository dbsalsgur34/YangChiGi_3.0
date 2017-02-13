using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ServerSide
{
    public class Match{
        int[] freeIds = new int[2];
        public int playerCount = 0;

        public Match()
        {

        }
        public Match(int freeId)
        {
            this.freeIds[playerCount] = freeId;
            playerCount++;
        }

        public int getFreeId(int index)
        {
            return this.freeIds[index];
        }

        public void addPlayer(int freeId)
        {
            this.freeIds[playerCount] = freeId;
            playerCount++;
        }
    }

}

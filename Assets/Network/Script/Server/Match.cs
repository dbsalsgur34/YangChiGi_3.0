using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ServerSide
{
    public class Match{
        int[] freeIds = new int[2];
        public int playerCount = 0;
		bool ready1 = false;
		bool ready2 = false;
		bool end1 = false;
		bool end2 = false;

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

		public void setReady(int id) {
			if(id == freeIds[0])
				ready1 = true;
			else if(id == freeIds[1])
				ready2 = true;

		}
		public void setEnd(int id) {
			if(id == freeIds[0])
				end1 = true;
			else if(id == freeIds[1])
				end2 = true;
		}
		public bool isAllReady() {
			return ready1 && ready2;
		}
		public bool isAllEnd() {
			return end1 && end2;
		}
    }

}

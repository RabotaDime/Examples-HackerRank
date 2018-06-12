using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IOChain;

namespace Examples_HackerRank.DataStructures.LinkedList
{
    class CECycleDetection : CExercise
    {
        public CECycleDetection () { ID = "Data Structures.Linked List.Cycle Detection"; }

        //    Входные данные:
        //-------------------------------------------------------------------------------
        //    


        //    Цель задачи:
        //-------------------------------------------------------------------------------
        //    


        //    Смысловое определение и решение:
        //-------------------------------------------------------------------------------
        //    


        //    Иллюстрация:
        //-------------------------------------------------------------------------------
        //    



        private class CListNode
        {
            public CListNode    Next;
            public int          Data;
        }



        private CListNode GenerateList (CIO io)
        {
            while (io.In(io.Number(out int n), io.InEOL()))
            {
            }
        }



        public override void Execute (CIO io)
        {
            var head = GenerateList(io);


        }
    }
}

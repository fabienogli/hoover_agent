using System.Collections.Generic;

namespace HooverAgent.Environment
{
    public class Mansion
    {
        private List<Object> Rooms;
        private bool _running;

        private bool Running
        {
            get => _running;
            set => _running = value;
        }

        public Mansion(int size)
        {
            Rooms = new List<Object>(size);
            for (var i = 0; i < size; i++)
            {
                Rooms.Add(Object.Nothing);
                //Generate jewels and dirt on first run 
            }

            Running = true;
        }


        public void Run()
        {
            while (Running)
            {
                if (ShouldGenerateDirt())
                {
                    GenerateDirt();
                }

                if (ShouldGenerateJewel())
                {
                    GenerateJewel();
                }
            }
        }

        private bool ShouldGenerateDirt()
        {
            //If some proba
            return false;
        }

        private void GenerateDirt()
        {
            //Random generation of dirt
        }


        private bool ShouldGenerateJewel()
        {
            //If some proba
            return false;
        }

        private void GenerateJewel()
        {
            //Random generation of Jewel
        }
    }
}



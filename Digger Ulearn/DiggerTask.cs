using System.Windows.Forms;
using static Digger.Game;
namespace Digger
{
    class Terrain : ICreature
    {
        CreatureCommand ICreature.Act(int x, int y)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
        }

        bool ICreature.DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        int ICreature.GetDrawingPriority()
        {
            return 1;
        }

        string ICreature.GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    class Player : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            switch (KeyPressed)
            {
                case Keys.Left:
                    if (x - 1 >= 0 && !(Map[x - 1, y] is Sack))
                        return new CreatureCommand { DeltaX = -1, DeltaY = 0, TransformTo = null };
                    else return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
                case Keys.Right:
                    if (x + 1 < MapWidth && !(Map[x + 1, y] is Sack))
                        return new CreatureCommand { DeltaX = 1, DeltaY = 0, TransformTo = null };
                    else return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
                case Keys.Down:
                    if (y + 1 < MapHeight && !(Map[x, y + 1] is Sack))
                        return new CreatureCommand { DeltaX = 0, DeltaY = 1, TransformTo = null };
                    else return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
                case Keys.Up:
                    if (y - 1 >= 0 && !(Map[x, y - 1] is Sack))
                        return new CreatureCommand { DeltaX = 0, DeltaY = -1, TransformTo = null };
                    else return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
                default: return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
            }
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Monster) { IsOver = true; return true; }
            if (conflictedObject is Sack) { IsOver = true; return true; }
            if (conflictedObject is Gold) Scores += 10;
            return false;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    class Sack : ICreature
    {
        public int Count = 0;
        public CreatureCommand Act(int x, int y)
        {
            if (y + 1 < MapHeight)
            {
                if (Map[x, y + 1] == null || (Count > 0 && Map[x, y + 1] is Player) || (Count > 0 && Map[x, y + 1] is Monster))
                {
                    Count++;
                    return new CreatureCommand { DeltaX = 0, DeltaY = 1, TransformTo = null };
                }
                else if (Count < 2) Count = 0;
            }
            if (Count > 1) return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = new Gold() };
            else return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            int xPlayer = 0;
            int yPlayer = 0;
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    if (Map[i, j] is Player)
                    {
                        IsOver = false;
                        xPlayer = i; yPlayer = j;
                    }
                }
            }
            if (!IsOver)
            {
                if (x > xPlayer)
                {
                    if (x - 1 >= 0 && !(Map[x - 1, y] is Sack) && !(Map[x - 1, y] is Terrain) && !(Map[x - 1, y] is Monster))
                        return new CreatureCommand { DeltaX = -1, DeltaY = 0, TransformTo = null };
                    else return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
                }
                if (x < xPlayer)
                {
                    if (x + 1 < MapWidth && !(Map[x + 1, y] is Sack) && !(Map[x + 1, y] is Terrain) && !(Map[x + 1, y] is Monster))
                        return new CreatureCommand { DeltaX = 1, DeltaY = 0, TransformTo = null };
                    else return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
                }
                if (y < yPlayer)
                {
                    if (y + 1 < MapHeight && !(Map[x, y+1] is Sack) && !(Map[x, y + 1] is Terrain) && !(Map[x, y + 1] is Monster))
                        return new CreatureCommand { DeltaX = 0, DeltaY = 1, TransformTo = null };
                    else return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
                }
                if (y > yPlayer)
                {
                    if (y - 1 >= 0 && !(Map[x, y - 1] is Sack) && !(Map[x, y - 1] is Terrain) && !(Map[x, y - 1] is Monster))
                        return new CreatureCommand { DeltaX = 0, DeltaY = -1, TransformTo = null };
                    else return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
                }
            }
            return new CreatureCommand { DeltaX = 0, DeltaY = 0, TransformTo = null };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster; 
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }
    }
}

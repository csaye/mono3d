using System;

namespace Test3D
{
    public class Map
    {
        private BlockType[,,] map;

        private const int width = 32;
        private const int height = 32;
        private const int length = 32;

        public Map()
        {
            // Initialize map
            map = new BlockType[width, length, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    for (int z = 0; z < length; z++)
                    {
                        // Calculate grass positions
                        int floorLevel = height / 4;
                        if (y < floorLevel)
                        {
                            int centered = Math.Abs((x + z) - (width / 2 + length / 2));
                            int centered01 = 1 - (centered / (width / 2 + length / 2));
                            if (centered01 * floorLevel < y)
                            {
                                map[x, y, z] = BlockType.Grass;
                            }
                        }
                    }
                }
            }
        }
    }
}

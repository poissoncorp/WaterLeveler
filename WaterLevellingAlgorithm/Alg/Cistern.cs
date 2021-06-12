using System;

namespace WaterLevellingAlgorithm.Alg
{
    public abstract class Cistern : IComparable
    {
        public uint BaseLevel { get; set; }
        public abstract double Volume();
        public abstract int Ceiling();
        public abstract double CalculateVolumeFromLevel(double level);
        public int CompareTo(object obj)
        {
            if (obj is Cistern cistern)
                return BaseLevel.CompareTo(cistern.BaseLevel);
            else
                throw new NotImplementedException();
        }
    }

    public class CuboidCistern : Cistern
    {
        public uint Height { get; set; }
        public uint Width { get; set; }
        public uint Depth { get; set; }
        public CuboidCistern(uint b, uint h, uint w, uint d)
        {
            BaseLevel = b;
            Height = h;
            Width = w;
            Depth = d;
        }
        public override double CalculateVolumeFromLevel(double level)
        {
            if (level > BaseLevel)
                if (level >= BaseLevel + Height)
                    return Volume();
                else
                    return (level - BaseLevel) * Width * Depth;
            return 0;
        }

        public override double Volume()
        {
            return Height * Width * Depth;
        }

        public override int Ceiling()
        {
            return (int)Height + (int)BaseLevel;
        }
    }

    public class CylinderCistern : Cistern
    {
        public uint Radius { get; set; }
        public uint Height { get; set; }
        public CylinderCistern(uint b, uint h, uint r)
        {
            BaseLevel = b;
            Height = h;
            Radius = r;
        }

        public override double CalculateVolumeFromLevel(double level)
        {
            if (level > BaseLevel)
                if (level >= BaseLevel + Height)
                    return Volume();
                else
                    return (Math.PI * Math.Pow(Radius, 2)) * (level - BaseLevel);
            return 0;
        }

        public override double Volume()
        {
            return Math.PI * Math.Pow(Radius, 2) * Height;
        }

        public override int Ceiling()
        {
            return (int)Height + (int)BaseLevel;
        }
    }

    public class SphereCistern : Cistern
    {
        public uint Radius { get; set; }
        public SphereCistern(uint b, uint r)
        {
            BaseLevel = b;
            Radius = r;
        }

        public override double CalculateVolumeFromLevel(double level)
        {
            if (level > BaseLevel)
                if (level >= BaseLevel + 2 * Radius)
                    return Volume();
                else
                {
                    double delta = (level - BaseLevel);
                    return ((Math.PI * delta) / 6) * (3 * (delta * (2 * Radius - delta)) + (delta * delta));
                }
            return 0;
        }

        public override double Volume()
        {
            return Math.PI * Math.Pow(Radius, 3) * ((double)4 / 3);
        }

        public override int Ceiling()
        {
            return 2 * (int)Radius + (int)BaseLevel;
        }
    }

    public class ConeCistern : Cistern
    {
        public uint Radius { get; set; }
        public uint Height { get; set; }
        public ConeCistern(uint b, uint h, uint r)
        {
            BaseLevel = b;
            Height = h;
            Radius = r;
        }

        public override double CalculateVolumeFromLevel(double level)
        {
            if (level > BaseLevel)
                if (level >= BaseLevel + 2 * Radius)
                    return Volume();
                else
                {
                    double delta = level - BaseLevel;
                    double upperRadius = (Height - delta) * Radius / Height;
                    return (double)1 / 3 * Math.PI * delta * (Radius * Radius + Radius * upperRadius + upperRadius * upperRadius);
                }
            return 0;
        }

        public override double Volume()
        {
            return (Math.PI * Math.Pow(Radius, 2) * Height) / 3;
        }

        public override int Ceiling()
        {
            return (int)Height + (int)BaseLevel;
        }
    }

}
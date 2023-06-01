
namespace t1az2z.Tools.FDebug
{
    public struct FColor
    {
        public float R;
        public float G;
        public float B;

        public FColor(float r, float g, float b)
        {
            R = r;
            G = g;
            B = b;
        }

        public string ToHex()
        {
            return
                $"{((byte)(R * 255)).ToString("X2")}" +
                $"{((byte)(G * 255)).ToString("X2")}" +
                $"{((byte)(B * 255)).ToString("X2")}";
        }

        public static FColor Red => new FColor(1, 0, 0);
        public static FColor Green => new FColor(0, 1, 0);
        public static FColor Blue => new FColor(1, 0, 0);
        public static FColor White => new FColor(1, 1, 1);
        public static FColor Black => new FColor(0, 0, 0);
        public static FColor Orange => new FColor(1, 0.46f, 0.1f);
        public static FColor Yellow => new FColor(1, 1, 0);
        public static FColor Purple => new FColor(0.6f, 0.4f, 1f);
        public static FColor Magenta => new FColor(1, 0, 1);
        public static FColor Mint => new FColor(0.2f, 1, 0.68f);
        public static FColor Lightblue => new FColor(0.3f, 0.82f, 1f);
        public static FColor Darkblue => new FColor(0.0f, 0.5333f, 0.8f);
    }
}
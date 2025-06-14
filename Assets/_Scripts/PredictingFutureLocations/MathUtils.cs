using UnityEngine;

public class MathUtils
{
    // Returns true if a valid (non-negative) solution is found
    public static bool SolveQuadratic(float a, float b, float c, out float t)
    {
        t = 0f;

        if (Mathf.Approximately(a, 0f))
        {
            // Linear case: bx + c = 0 => x = -c / b
            if (!Mathf.Approximately(b, 0f))
            {
                float root = -c / b;
                if (root >= 0f)
                {
                    t = root;
                    return true;
                }
            }
            return false;
        }

        float discriminant = b * b - 4f * a * c;

        if (discriminant < 0f)
        {
            return false;
        }

        float sqrtD = Mathf.Sqrt(discriminant);
        float t1 = (-b - sqrtD) / (2f * a);
        float t2 = (-b + sqrtD) / (2f * a);

        // Pick the smallest non-negative root
        if (t1 >= 0f && t2 >= 0f)
        {
            t = Mathf.Min(t1, t2);
            return true;
        }
        else if (t1 >= 0f)
        {
            t = t1;
            return true;
        }
        else if (t2 >= 0f)
        {
            t = t2;
            return true;
        }

        return false;
    }
}

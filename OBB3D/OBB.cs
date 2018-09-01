using UnityEngine;

public class OBB : MonoBehaviour
{
    public Vector3 size;
    public Color gizmosColor = Color.white;

    Vector3 P0 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(-size.x * 0.5f, -size.y * 0.5f, -size.z * 0.5f)); } }
    Vector3 P1 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(size.x * 0.5f, -size.y * 0.5f, -size.z * 0.5f)); } }
    Vector3 P2 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(size.x * 0.5f, size.y * 0.5f, -size.z * 0.5f)); } }
    Vector3 P3 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(-size.x * 0.5f, size.y * 0.5f, -size.z * 0.5f)); } }

    Vector3 P4 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(-size.x * 0.5f, -size.y * 0.5f, size.z * 0.5f)); } }
    Vector3 P5 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(size.x * 0.5f, -size.y * 0.5f, size.z * 0.5f)); } }
    Vector3 P6 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(size.x * 0.5f, size.y * 0.5f, size.z * 0.5f)); } }
    Vector3 P7 { get { return transform.localToWorldMatrix.MultiplyPoint3x4(new Vector3(-size.x * 0.5f, size.y * 0.5f, size.z * 0.5f)); } }

    Vector3 XAxis { get { return transform.right; } }
    Vector3 YAxis { get { return transform.up; } }
    Vector3 ZAxis { get { return transform.forward; } }


    public bool Intersects(OBB other)
    {
        var isNotIntersect = false;
        isNotIntersect |= ProjectionIsNotIntersect(this, other, XAxis);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, YAxis);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, ZAxis);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, other.XAxis);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, other.YAxis);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, other.ZAxis);

        isNotIntersect |= ProjectionIsNotIntersect(this, other, Vector3.Cross(XAxis, other.XAxis).normalized);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, Vector3.Cross(XAxis, other.YAxis).normalized);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, Vector3.Cross(XAxis, other.ZAxis).normalized);

        isNotIntersect |= ProjectionIsNotIntersect(this, other, Vector3.Cross(YAxis, other.XAxis).normalized);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, Vector3.Cross(YAxis, other.YAxis).normalized);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, Vector3.Cross(YAxis, other.ZAxis).normalized);

        isNotIntersect |= ProjectionIsNotIntersect(this, other, Vector3.Cross(ZAxis, other.XAxis).normalized);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, Vector3.Cross(ZAxis, other.YAxis).normalized);
        isNotIntersect |= ProjectionIsNotIntersect(this, other, Vector3.Cross(ZAxis, other.ZAxis).normalized);

        return isNotIntersect ? false : true;
    }

    bool ProjectionIsNotIntersect(OBB x, OBB y, Vector3 axis)
    {
        var x_p0 = GetSignProjectValue(x.P0, axis);
        var x_p1 = GetSignProjectValue(x.P1, axis);
        var x_p2 = GetSignProjectValue(x.P2, axis);
        var x_p3 = GetSignProjectValue(x.P3, axis);
        var x_p4 = GetSignProjectValue(x.P4, axis);
        var x_p5 = GetSignProjectValue(x.P5, axis);
        var x_p6 = GetSignProjectValue(x.P6, axis);
        var x_p7 = GetSignProjectValue(x.P7, axis);

        var y_p0 = GetSignProjectValue(y.P0, axis);
        var y_p1 = GetSignProjectValue(y.P1, axis);
        var y_p2 = GetSignProjectValue(y.P2, axis);
        var y_p3 = GetSignProjectValue(y.P3, axis);
        var y_p4 = GetSignProjectValue(y.P4, axis);
        var y_p5 = GetSignProjectValue(y.P5, axis);
        var y_p6 = GetSignProjectValue(y.P6, axis);
        var y_p7 = GetSignProjectValue(y.P7, axis);

        var xMin = Mathf.Min(x_p0, Mathf.Min(x_p1, Mathf.Min(x_p2, Mathf.Min(x_p3, Mathf.Min(x_p4, Mathf.Min(x_p5, Mathf.Min(x_p6, x_p7)))))));
        var xMax = Mathf.Max(x_p0, Mathf.Max(x_p1, Mathf.Max(x_p2, Mathf.Max(x_p3, Mathf.Max(x_p4, Mathf.Max(x_p5, Mathf.Max(x_p6, x_p7)))))));
        var yMin = Mathf.Min(y_p0, Mathf.Min(y_p1, Mathf.Min(y_p2, Mathf.Min(y_p3, Mathf.Min(y_p4, Mathf.Min(y_p5, Mathf.Min(y_p6, y_p7)))))));
        var yMax = Mathf.Max(y_p0, Mathf.Max(y_p1, Mathf.Max(y_p2, Mathf.Max(y_p3, Mathf.Max(y_p4, Mathf.Max(y_p5, Mathf.Max(y_p6, y_p7)))))));

        if (yMin >= xMin && yMin <= xMax) return false;
        if (yMax >= xMin && yMax <= xMax) return false;
        if (xMin >= yMin && xMin <= yMax) return false;
        if (xMax >= yMin && xMax <= yMax) return false;

        return true;
    }

    float GetSignProjectValue(Vector3 point, Vector3 axis)
    {
        var projectPoint = Vector3.Project(point, axis);
        var result = projectPoint.magnitude * Mathf.Sign(Vector3.Dot(projectPoint, axis));

        return result;
    }

    void OnDrawGizmos()
    {
        var cacheMatrix = Gizmos.matrix;
        var cacheColor = Gizmos.color;

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(Vector3.zero, size);
        Gizmos.matrix = cacheMatrix;
        Gizmos.color = cacheColor;
    }
}
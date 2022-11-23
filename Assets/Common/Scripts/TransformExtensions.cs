using System.Collections.Generic;
using UnityEngine;

public static class TransformExtensions
{
    public static T[] FindObjectsOfTypeInChildren<T>(this Transform parent) where T : UnityEngine.Object
    {
        List<T> objects = new List<T>();
        if (parent.TryGetComponent(out T component)) objects.Add(component);
        for (int i = 0; i < parent.childCount; i++) objects.AddRange(FindObjectsOfTypeInChildren<T>(parent.GetChild(i)));
        return objects.ToArray();
    }

    public static GameObject[] FindGameObjectsWithNameInChildren(this Transform parent, string name)
    {
        List<GameObject> objects = new List<GameObject>();
        if (parent.gameObject.name == name) 
            objects.Add(parent.gameObject);
        for (int i = 0; i < parent.childCount; i++) 
            objects.AddRange(FindGameObjectsWithNameInChildren(parent.GetChild(i), name));
        return objects.ToArray();
    }

    public static T GetNearestObject<T>(IEnumerable<T> objects, Vector3 position) where T : Component
    {
        T targetObject = null;

        float minDistance = 10000f;
        foreach (T @object in objects)
        {
            float distance = Vector3.Distance(@object.transform.position, position);

            if (distance < minDistance)
            {
                targetObject = @object;
                minDistance = distance;
            }
        }

        return targetObject;
    }

    public static Vector3 LerpByStep(this Vector3 vector, Vector3 target, float step)
    {
        Vector3 targetOffcet = target - vector;
        Vector3 offcet = targetOffcet.Clamp(-step, step);
        return vector + offcet;
    }

    public static Quaternion GetScreenRotationFromGlobalDirection(Vector3 playerPosition, Vector3 targetPosition, Camera camera)
    {
        targetPosition = playerPosition + (targetPosition - playerPosition).normalized;
        Vector2 screenDirection = camera.WorldToScreenPoint(targetPosition) - camera.WorldToScreenPoint(playerPosition);
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, screenDirection.normalized);

        return rotation;
    }

    public static float GetMagnitudeFromRotation(this float zAngle, Camera camera) =>
        new Vector2(Mathf.Sin(Mathf.Deg2Rad * zAngle) * camera.pixelWidth / 2, Mathf.Cos(Mathf.Deg2Rad * zAngle) * camera.pixelHeight / 2).magnitude;


    //public static void WorldDirectionToScreen(Vector3 startPosition, Vector3 targetDirection, Camera camera, 
    //    out Vector2 screenPosition, out Quaternion screenRotation, float boundsOffcet)
    //{
    //    Vector2 screenDirection = camera.WorldToScreenPoint(startPosition + (targetDirection - startPosition).normalized)
    //        - camera.WorldToScreenPoint(startPosition);

    //    screenPosition = (screenDirection)

    //}

    public static Vector3 Abs(this Vector3 vector)
    {
        return new Vector3(Mathf.Abs(vector.x), Mathf.Abs(vector.y), Mathf.Abs(vector.z));
    }

    public static Vector3 Clamp(this Vector3 vector, Vector3 min, Vector3 max)
    {
        return new Vector3(Mathf.Clamp(vector.x, min.x, max.x), Mathf.Clamp(vector.y, min.y, max.y), Mathf.Clamp(vector.z, min.z, max.z));
    }
    public static Vector3 Clamp(this Vector3 vector, float min, float max)
    {
        return new Vector3(Mathf.Clamp(vector.x, min, max), Mathf.Clamp(vector.y, min, max), Mathf.Clamp(vector.z, min, max));
    }

    public static Vector3 XY(this Vector3 vector) => new Vector3(vector.x, vector.y, 0f);
    public static Vector3 XZ(this Vector3 vector) => new Vector3(vector.x, 0f, vector.z);
    public static Vector3 YZ(this Vector3 vector) => new Vector3(0f, vector.y, vector.z);
    public static Vector3 X(this Vector3 vector) => new Vector3(vector.x, 0f, 0f);
    public static Vector3 Y(this Vector3 vector) => new Vector3(0f, vector.y, 0f);
    public static Vector3 Z(this Vector3 vector) => new Vector3(0f, 0f, vector.z);


    public static Vector3 SetX(this Vector3 vector, float value) => new Vector3(value, vector.y, vector.z);
    public static Vector3 SetY(this Vector3 vector, float value) => new Vector3(vector.x, value, vector.z);
    public static Vector3 SetZ(this Vector3 vector, float value) => new Vector3(vector.x, vector.y, value);




    public static Vector3 GetGlobalPosition(this Transform transform)
    {
        if (transform.parent == null)
            return transform.localPosition;
        else return (transform.parent.localRotation * transform.localPosition) + transform.parent.GetGlobalPosition();
    }

    public static Quaternion GetGlobalRotation(this Transform transform)
    {
        if (transform.parent == null)
            return transform.localRotation;
        else return transform.localRotation * transform.parent.GetGlobalRotation();
    }

    public static Vector3 GetGlobalScale(this Transform transform)
    {
        Vector3 scale = transform.rotation * transform.localScale;

        if (transform.parent == null)
            return scale.Abs();
        else return Vector3.Scale(scale.Abs(), transform.parent.GetGlobalScale());
    }

}

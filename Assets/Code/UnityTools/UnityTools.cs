using System;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class UnityTools {

    public static GameObject ClosestWithTag(GameObject target, string tag) {
        GameObject value = null;
        GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);
        float closestDist = Mathf.Infinity;

        foreach (GameObject obj in objectsWithTag) {
            float testingDist = Vector3.Distance(target.transform.position, obj.transform.position);
            if (testingDist < closestDist) {
                closestDist = testingDist;
                value = obj;
            }
        }

        return value;
    }

    public static List<Vector3> pointsInsideEllipse(Vector3 radii, int divisor, Vector3 multiplier) {
        List<Vector3> values = new List<Vector3>();
        Vector2 angles = new Vector2(1,1);
        Debug.Log("PointsInsideEllipse called.");


        for (float i = -Mathf.Pow(Mathf.PI * radii.x, 2); i <= Mathf.Pow(Mathf.PI * radii.x, 2); i++) {
            for (float j = -Mathf.Pow(Mathf.PI * radii.y, 2); j <= Mathf.Pow(Mathf.PI * radii.y, 2); j++) {
                for (float k = -Mathf.Pow(Mathf.PI * radii.z, 2); k <= Mathf.Pow(Mathf.PI * radii.z, 2); k++) {
                    values.Add(new Vector3(i, j, k));
                    Debug.Log("Vale Added :" + new Vector3(i, j, k));
                }
            }
        }


            /*
            for (float i = -((radii.x * Mathf.Cos(angles.x) * Mathf.Sin(angles.y)) / divisor); i <= (radii.x * Mathf.Cos(angles.x) * Mathf.Sin(angles.y)) / divisor; i++) {
                for (float j = -((radii.y * Mathf.Sin(angles.x) * Mathf.Sin(angles.y)) / divisor); j <= (radii.y * Mathf.Sin(angles.x) * Mathf.Sin(angles.y)) / divisor; j++) {
                    angles.x = Vector2.Angle(new Vector2(0, 0), new Vector2(i, j));
                    Debug.Log("Angles.x : " + angles.x);
                    for (float k = -((radii.z * Mathf.Cos(angles.y)) / divisor); k <= (radii.z * Mathf.Cos(angles.y)) / divisor; k++) {
                        angles.y = Vector2.Angle(new Vector2(0, 0), new Vector2(j, k));
                        Debug.Log("Angle.y : " + angles.y);
                        Vector3 testPos = new Vector3(i*multiplier.x, j*multiplier.y, k*multiplier.z);
                        if (!values.Contains(testPos)) {
                            values.Add(testPos);
                            Debug.Log("Added value: " + new Vector3(i * multiplier.x, j * multiplier.y, k * multiplier.z));
                        }    
                    }
                }
            }
            */
            return values;
    }

    public static Vector3 ClosestAngle(Vector3 direction, Vector3[] angles) {
        Vector3 value = new Vector3();
        float closestAngl = Mathf.Infinity;
        foreach (Vector3 vec3 in angles) {
            float testingAngl = Mathf.Abs((vec3 - direction).sqrMagnitude);
            if (testingAngl < closestAngl) {
                closestAngl = testingAngl;
                value = vec3;
            }
        }

        return value;
    }

    public static List<Vector3> PointsInsideVolume(Vector3 length, int divisor, Vector3 multiplier) {
        List<Vector3> values = new List<Vector3>();
        for (float i = -length.x / divisor; i < length.x / divisor; i++) {
            for (float j = -length.y / divisor; j < length.y / divisor; j++) {
                for (float k = -length.z / divisor; k < length.z / divisor; k++) {
                    values.Add(new Vector3(j*multiplier.x, k*multiplier.y, i*multiplier.z));
                }
            }
        }
        return values;
    }

    public static Vector3 Ceiling(Vector3 position, int places) {
        Vector3 value = new Vector3();
        float x = position.x, y = position.y, z = position.z;

        if (x > 0) { x = (float)Math.Ceiling(x); } else { x = (float)Math.Floor(x); }
        if (y > 0) { y = (float)Math.Ceiling(y); } else { y = (float)Math.Floor(y); }
        if (z > 0) { z = (float)Math.Ceiling(z); } else { z = (float)Math.Floor(z); }

        x = (float)Math.Round(x / places, 0, MidpointRounding.AwayFromZero) * places;
        y = (float)Math.Round(y / places, 0, MidpointRounding.AwayFromZero) * places;
        z = (float)Math.Round(z / places, 0, MidpointRounding.AwayFromZero) * places;

        value = new Vector3(x, y, z);
        return value;
    }

    public static GameObject GetMasterController() {
        return GameObject.FindGameObjectWithTag("GameController");
    }
}
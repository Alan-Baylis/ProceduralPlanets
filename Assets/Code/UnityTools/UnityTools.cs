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

    public static List<Vector3> PointsInsideSphere(int radius, int divisor, int multiplier) {
        List<Vector3> values = new List<Vector3>();

        for (int i = -radius; i <= radius; i++) {
            for (int j = -radius; j <= radius; j++) {
                for (int k = -radius; k <= radius; k++) {
                    if (i * i + j * j + k * k <= radius * radius) {
                        values.Add(new Vector3(j * multiplier, k * multiplier, i * multiplier));
                    }
                }
            }
        }

        return values;
    }

    public static List<Vector3> PointsInsideEllipse(Vector3 radii, int divisor, Vector3 multiplier) {
        List<Vector3> values = new List<Vector3>();

        for (float i = -radii.x / divisor; i < radii.x / divisor; i++) {
            for (float j = -radii.y / divisor; j < radii.y / divisor; j++) {
                for (float k = -radii.z / divisor; k < radii.z / divisor; k++) {
                    if (((i / radii.x) * (i / radii.x) + (j / radii.y) * (j / radii.y) + (k / radii.z) * (k / radii.z)) <= 1) {
                        values.Add(new Vector3(j * multiplier.x, k * multiplier.y, i * multiplier.z));
                    }
                }
            }
        }
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
                    values.Add(new Vector3(j * multiplier.x, k * multiplier.y, i * multiplier.z));
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
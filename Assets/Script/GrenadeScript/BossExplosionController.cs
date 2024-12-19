using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class BossExplosionController : MonoBehaviour
{
    [System.Serializable]
    public class ExplosionPoint
    {
        public GameObject explosionPrefab; // Prefab hiệu ứng cháy nổ
        public float delay; // Thời gian chờ trước khi kích hoạt
        public Transform explosionTransform; // Vị trí để tạo hiệu ứng
        public string explosionSoundName; // Âm thanh nổ
    }

    [Header("Explosion Settings")]
    public List<ExplosionPoint> explosionPoints; // Danh sách các vị trí nổ

    public void TriggerExplosions()
    {
        if (explosionPoints == null || explosionPoints.Count == 0)
        {
            Debug.LogWarning("Explosion points list is empty!");
            return;
        }
        StartCoroutine(TriggerExplosionSequence());
    }

    private IEnumerator TriggerExplosionSequence()
    {
        foreach (var point in explosionPoints)
        {
            // Kiểm tra null cho từng phần tử
            if (point.explosionPrefab == null || point.explosionTransform == null)
            {
                Debug.LogWarning($"Missing explosionPrefab or explosionTransform at point with delay {point.delay}");
                continue;
            }

            // Instantiate hiệu ứng nổ tại vị trí của GameObject con
            Instantiate(point.explosionPrefab, point.explosionTransform.position, Quaternion.identity);

            // Phát âm thanh nổ nếu có
            if (!string.IsNullOrEmpty(point.explosionSoundName))
            {
                ManagerAudioSound.Instance.PlayExplodeSound(point.explosionSoundName);
                Debug.Log($"Playing sound: {point.explosionSoundName} at {point.explosionTransform.position}");
            }

            // Chờ trước khi tiếp tục tới điểm nổ tiếp theo
            yield return new WaitForSeconds(point.delay);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;

public class GameObjectMover : MonoBehaviour
{
    public enum EaseType
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut
    }

    public void Move(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong, EaseType easeType)
    {
        StartCoroutine(MoveCoroutine(gameObject, begin, end, time, pingpong, easeType));
    }

    // 协程
    private IEnumerator MoveCoroutine(GameObject gameObject, Vector3 begin, Vector3 end, float time, bool pingpong, EaseType easeType)
    {
        // 初始位置
        Vector3 startPosition = begin;
        // 目标位置
        Vector3 targetPosition = end;
        // 移动时间
        float moveTime = 0f;
        // 移动速度
        float speed = 1f / time;

        while (true)
        {
            // 计算当前时间的插值
            moveTime += Time.deltaTime;
            float t = Mathf.Clamp01(moveTime * speed);

            // 根据easeType选择不同的插值方式
            float ease = 0f;
            switch (easeType)
            {
                case EaseType.Linear:
                    ease = t; // Linear
                    break;
                case EaseType.EaseIn:
                    ease = t * t; // EaseIn
                    break;
                case EaseType.EaseOut:
                    ease = 1f - Mathf.Pow(1f - t, 2); // EaseOut
                    break;
                case EaseType.EaseInOut:
                    if (t < 0.5f)
                        ease = 2f * t * t;
                    else
                        ease = 1f - Mathf.Pow(-2f * t + 2f, 2) / 2f; // EaseInOut
                    break;
            }

            gameObject.transform.position = Vector3.Lerp(startPosition, targetPosition, ease);

            // 检查是否到达目标位置
            if (t >= 1f)
            {
                if (pingpong)
                {
                    // 反转起始和目标位置
                    Vector3 temp = startPosition;
                    startPosition = targetPosition;
                    targetPosition = temp;
                    // 重置移动时间
                    moveTime = 0f;
                }
                else
                {
                    // 如果不是pingpong模式，退出循环
                    yield break;
                }
            }

            // 等待下一帧
            yield return null;
        }
    }
}

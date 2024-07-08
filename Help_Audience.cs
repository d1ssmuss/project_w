using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class Help_Audience : MonoBehaviour
{
    public RectTransform imageRectTransform_a; // Присвойте сюда ссылку на RectTransform вашего Image
    public RectTransform imageRectTransform_b;
    public RectTransform imageRectTransform_c;
    public RectTransform imageRectTransform_d;
    public Text procent_a;
    public Text procent_b;
    public Text procent_c;
    public Text procent_d;
    public void SetHeight()
    {
        imageRectTransform_a.pivot = new Vector2(0.5f, 1);

        // Получаем текущий размер и позицию
        Vector2 sizeDelta = imageRectTransform_a.sizeDelta;
        Vector2 anchoredPosition = imageRectTransform_a.anchoredPosition;

        // Изменяем только высоту
        sizeDelta.y += 10;

        // Применяем изменения
        imageRectTransform_a.sizeDelta = sizeDelta;
        // Оставляем верхнюю часть на месте
        imageRectTransform_a.anchoredPosition = anchoredPosition;
    }


    private const int totalQuestions = 15;  // Всего 15 вопросов
    private const float initialCorrectProbability = 0.8f;  // Начальная вероятность правильного ответа (80%)
    private const float finalCorrectProbability = 0.25f;  // Вероятность правильного ответа на последний вопрос (25%)
    private System.Random random = new System.Random(); // Для генерации случайных чисел

    void Start()
    {
        for (int i = 1; i <= totalQuestions; i++)
        {
            float correctProbability = CalculateCorrectProbability(i);
            (float wrongA, float wrongB, float wrongC) = CalculateRandomWrongProbabilities(correctProbability);

            // Округляем значения до целых процентов
            int correctPercent = Mathf.RoundToInt(correctProbability * 100);
            int wrongAPercent = Mathf.RoundToInt(wrongA * 100);
            int wrongBPercent = Mathf.RoundToInt(wrongB * 100);
            int wrongCPercent = Mathf.RoundToInt(wrongC * 100);

            // Проверяем, чтобы сумма была равна 100%
            int totalPercent = correctPercent + wrongAPercent + wrongBPercent + wrongCPercent;
            if (totalPercent != 100)
            {
                int difference = 100 - totalPercent;
                correctPercent += difference; // Корректируем правильный ответ, чтобы сумма была 100%
            }

            Debug.Log($"Вопрос {i}: Вероятность правильного ответа: {correctPercent}%, " +
                      $"вероятность неправильного ответа A: {wrongAPercent}%, " +
                      $"вероятность неправильного ответа B: {wrongBPercent}%, " +
                      $"вероятность неправильного ответа C: {wrongCPercent}%");
        }
    }

    private float CalculateCorrectProbability(int questionNumber)
    {
        float k = Mathf.Log(finalCorrectProbability / initialCorrectProbability) / (totalQuestions - 1);
        return initialCorrectProbability * Mathf.Exp(k * (questionNumber - 1));
    }

    private (float, float, float) CalculateRandomWrongProbabilities(float correctProbability)
    {
        float remainingProbability = 1.0f - correctProbability;

        // Генерируем случайные веса для неправильных ответов
        float weightA = (float)random.NextDouble();
        float weightB = (float)random.NextDouble();
        float weightC = (float)random.NextDouble();
        float totalWeight = weightA + weightB + weightC;

        // Распределяем оставшуюся вероятность
        float wrongA = remainingProbability * (weightA / totalWeight);
        float wrongB = remainingProbability * (weightB / totalWeight);
        float wrongC = remainingProbability * (weightC / totalWeight);

        return (wrongA, wrongB, wrongC);
    }
}

using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CurrencyLayer
{
    public class CurrencyLayerView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _exchangeText;
        [SerializeField] private Button _updateButton;

        private static string EXCHANGE_TEXT = "Курс валют USD/EUR: ";
        private static int WAIT_TIME = 300000;

        private CurrencyLayerAPI _currencyLayerAPI;

        [Inject]
        private void Construct(CurrencyLayerAPI currencyLayerAPI)
        {
            _currencyLayerAPI = currencyLayerAPI;
            Bind();
            ChangeExchangeRate(true);
        }

        private void OnDestroy()
        {
            Expose();
        }

        private void Bind()
        {
            _updateButton.onClick.AddListener(() => ChangeExchangeRate(false));
        }

        private void Expose()
        {
            _updateButton.onClick.RemoveListener(() => ChangeExchangeRate(false));
        }

        private async void ChangeExchangeRate(bool isDelay)
        {
            float exchangeRate = await _currencyLayerAPI.GetExchangeRate();
            _exchangeText.text = $"{EXCHANGE_TEXT}{exchangeRate}";

            if (isDelay)
            {
                await Task.Delay(WAIT_TIME);
                ChangeExchangeRate(true);
            }
        }
    }
}
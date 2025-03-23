namespace Core.Models
{
    public class Ticker
    {
        /// <summary>
        /// Валютная пара
        /// </summary>
        public string Pair { get; set; }

        /// <summary>
        /// Цена последней самой высокой заявки на покупку
        /// </summary>
        public decimal Bid { get; set; }

        /// <summary>
        /// Сумма 25 самых высоких заявок на покупку
        /// </summary>
        public decimal BidSize { get; set; }

        /// <summary>
        /// Цена последней самой низкой заявки на продажу
        /// </summary>
        public decimal Ask { get; set; }

        /// <summary>
        /// Сумма 25 самых низких заявок на продажу
        /// </summary>
        public decimal AskSize { get; set; }

        /// <summary>
        /// Изменение цены за день
        /// </summary>
        public decimal DailyChange { get; set; }

        /// <summary>
        /// Относительное изменение цены за день в %
        /// </summary>
        public decimal DailyChangeRelative { get; set; }

        /// <summary>
        /// Цена последней сделки
        /// </summary>
        public decimal LastPrice { get; set; }

        /// <summary>
        /// Объем торгов за день
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Максимальная цена за день
        /// </summary>
        public decimal High { get; set; }

        /// <summary>
        /// Минимальная цена за день
        /// </summary>
        public decimal Low { get; set; }
    }
}

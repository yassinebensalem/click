using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD.Domain.Specifications
{
    /// <summary>
    /// Filtre d'obtention du nombre de ventes.
    /// </summary>
    public class DashboardFilter
    {
        /// <summary>
        /// Id du livre dont on veut retourner le nombre de ventes
        /// </summary>
        public string BookId { get; set; }

        /// <summary>
        /// Id de la maison d'édition dont on veut retourner le nombre de livres vendus
        /// </summary>
        public string EditorId { get; set; }

        /// <summary>
        /// Id de l'auteur dont on veut retourner le nombre de livres vendus
        /// </summary>
        public string AuthorId { get; set; }
        /// <summary>
        /// Id de la langue dont on veut retourner le nombre de livres vendus
        /// </summary>
        public double? LanguageId { get; set; }
        /// <summary>
        /// Date à laquelle on veut retourner le nombre de livres vendus
        /// </summary>
        public DateTime? Date { get; set; }
        /// <summary>
        /// Date début de la période dont on veut retourner le nombre de livres vendus
        /// </summary>
        public DateTime? FromDate { get; set; }
        /// <summary>
        /// Date de fin de la période dont on veut retourner le nombre de livres vendus
        /// </summary>
        public DateTime? ToDate { get; set; }
    }
}

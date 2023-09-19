using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DDD.Application.Enum.Constants;
using static DDD.Domain.Common.Constants.State;

namespace DDD.Application.Specifications
{
    /// <summary>
    /// Obtention de la liste filtrée des livres.
    /// </summary>
    public class PagedBooks
    {
        /// <summary>
        /// Index de la page à retourner.
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// Nombre de livres par page.
        /// </summary>
        public int PageSize { get; set; } = 12;

        /// <summary>
        /// Id de l'éditeur dont on veut retourner la liste des livres
        /// </summary>
        public string EditorId { get; set; }

        /// <summary>
        /// Liste des catégories des livres qu'on veut retourner
        /// </summary>
        public List<string> Categories { get; set; }
        /// <summary>
        /// Liste des langues des livres qu'on veut retourner
        /// </summary>
        public List<int> Languages { get; set; }
        /// <summary>
        /// Liste des langues des auteurs qu'on veut retourner
        /// </summary>
        public List<string> Authors { get; set; }
        /// <summary>
        /// Dire si on veut charger uniquement les livres en promotion.
        /// </summary>
        public bool IsPromotedBook { get; set; }
        /// <summary>
        /// Type de promotion : free/disount.
        /// </summary>
        public PromotionType PromotionType { get; set; }
        /// <summary>
        /// Type de recherche par mot clé.
        /// </summary>
        public SearchEnum SearchKeyType { get; set; }
        /// <summary>
        /// Texte de recherche par mot clé.
        /// </summary>
        public string SearchKeyText { get; set; }
    }
}

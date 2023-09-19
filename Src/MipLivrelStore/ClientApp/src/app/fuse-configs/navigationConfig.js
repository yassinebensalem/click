import React from 'react';
import { createMuiTheme } from '@material-ui/core';

import i18next from 'i18next';
import ar from './navigation-i18n/ar';
import en from './navigation-i18n/en';
import tr from './navigation-i18n/tr';

const navigationConfig = [
	{
		id: 'applications',
		title: 'Applications',
		translate: 'Menu',
		type: 'group',
		url: '/admin',
		icon: 'apps',
		children: [
			{
				id: 'Dashboard',
				title: 'Dashboard',
				type: 'item',
				icon: 'dashboard',
				url: '/admin',
				variant: 'primary'
			},
			{
				id: 'Author',
				title: 'Utilisateurs',
				type: 'collapse',
				icon: 'person',
				url: '/author/list',
				children: [
					{
						id: 'Author',
						title: 'Gestion des auteurs sans ME',
						type: 'item',
						url: '/author/list'
					},
					{
						id: 'Editor',
						title: 'Gestion des éditeurs',
						type: 'item',
						url: '/editor/list'
					},
					{
						id: 'Subscriber',
						title: 'Gestion	 des clients',
						type: 'item',
						url: '/subscriber/list'
					}
				]
			},
			{
				id: 'ask_editors',
				title: "Demandes d'adhésion",
				type: 'collapse',
				icon: 'person_add',
				url: '/ask/editors',
				children: [
					{
						id: 'ask_editors',
						title: 'Editeurs',
						type: 'item',
						url: '/ask/editors'
					},
					{
						id: 'ask_authors',
						title: 'Auteurs',
						type: 'item',
						url: '/ask/authors'
					}
				]
			},
			{
				id: 'author_without_account',
				title: 'Auteurs via ME',
				type: 'item',
				icon: 'person',
				url: '/authorWithoutAccount/list'
			},
			{
				id: 'categories',
				title: 'Catégories',
				type: 'collapse',
				icon: 'category',
				url: '/categories',
				children: [
					{
						id: 'categories',
						title: 'Gestion des catégories',
						type: 'item',
						url: '/categories'
					},
					{
						id: 'packs',
						title: 'Gestion des packs',
						type: 'item',
						url: '/categories/packs'
					}
				]
			},
			{
				id: 'Books',
				title: 'Livres',
				type: 'collapse',
				icon: 'menu_book',
				url: '/book/list',
				children: [
					{
						id: 'add-book',
						title: 'Ajouter un livre',
						type: 'item',
						url: '/book/new'
					},
					{
						id: 'book-list',
						title: 'Liste des livres',
						type: 'item',
						url: '/book/list'
					},
					{
						id: 'comments',
						title: 'Commentaires',
						type: 'item',
						url: '/book/comments'
					}
				]
			},
			{
				id: 'Prizes',
				title: 'Prix',
				type: 'collapse',
				icon: 'emoji_events',
				url: '/bookPrize/list',
				children: [
					{
						id: 'prize-list',
						title: 'Liste des prix',
						type: 'item',
						url: '/bookPrize/list'
					},
					{
						id: 'add-prize',
						title: 'Ajouter un prix',
						type: 'item',
						url: '/bookPrize/new'
					}
				]
			},
			{
				id: 'Orders',
				title: 'Factures',
				type: 'item',
				icon: 'receipt_long',
				url: '/bills/list'
			},
			{
				id: 'C',
				title: 'Communautés',
				type: 'collapse',
				icon: 'emoji_events',
				url: '/communities/list',
				children: [
					{
						id: 'community-management',
						title: 'Gestion des communauté',
						type: 'item',
						url: '/communities/list'
					},
					{
						id: 'add-community',
						title: 'Ajouter une communauté',
						type: 'item',
						url: '/community/new'
					}
				]
			},
			{
				id: 'CMS',
				title: 'CMS',
				type: 'collapse',
				icon: 'adb',
				url: '/cms/slides',
				children: [
					{
						id: 'slides',
						title: 'slides',
						type: 'item',
						url: '/cms/slides'
					},
					{
						id: 'adversiting',
						title: 'Panneaux Publicitaires',
						type: 'item',
						url: '/cms/advertisement'
					},
					{
						id: 'Offers',
						title: 'Offres',
						type: 'item',
						url: '/cms/offers'
					},
					{
						id: 'Blogs',
						title: 'Blogs',
						type: 'item',
						url: '/cms/blogs'
					}
				]
			},
			{
				id: 'P',
				title: 'Promotions',
				type: 'collapse',
				icon: 'campaign',
				url: '/promotion/list',
				children: [
					{
						id: 'promotion-list',
						title: 'Liste des Promotions',
						type: 'item',
						url: '/promotion/list'
					},
					{
						id: 'Add-promotion',
						title: 'Ajouter une Promotion',
						type: 'item',
						url: '/promotion/new'
					},
				]
			},
		]
	}
];

export default navigationConfig;

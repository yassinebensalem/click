import i18next from 'i18next';
import Dashboard from './Dashboard';
import NewBook from '../../fuse-layouts/books/NewBook';
import BooksList from '../../fuse-layouts/books/BooksList';
import UpdateBook from '../../fuse-layouts/books/UpdateBook';
import Authors from '../../fuse-layouts/authors/Authors';
import NewAuthor from '../../fuse-layouts/authors/NewAuthor';
import Subscribers from '../../fuse-layouts/subscribers/Subscribers';
import Packs from '../../fuse-layouts/packs/Packs';
import Comments from '../../fuse-layouts/books/comments';
import ReplyComments from '../../fuse-layouts/books/ReplyComments';
import Bills from '../../fuse-layouts/bills/Bills';
import Slides from '../../fuse-layouts/cms/Slides';
import NewSlide from '../../fuse-layouts/cms/NewSlide';
import Advertisement from '../../fuse-layouts/cms/Advertisement';
import Offers from '../../fuse-layouts/cms/Offers';
import Blogs from '../../fuse-layouts/cms/Blogs';
import NewSubscriber from '../../fuse-layouts/subscribers/NewSubscriber';
import en from './i18n/en';
import tr from './i18n/tr';
import ar from './i18n/ar';
import Editors from 'app/fuse-layouts/editors/Editors';
import NewEditor from 'app/fuse-layouts/editors/NewEditor';
import Categories from 'app/fuse-layouts/categories/Categories';
import EditAuthor from 'app/fuse-layouts/authors/EditAuthor';
import EditSubscriber from 'app/fuse-layouts/subscribers/EditSubscriber';
import EditorsJoin from '../../fuse-layouts/askToJoin/editorsJoin';
import AuthorsJoin from '../../fuse-layouts/askToJoin/authorsJoin';
import EditEditor from 'app/fuse-layouts/editors/EditEditor';
import AuthorsWithoutAccount from '../../fuse-layouts/authors/AuthorsWithoutAccount';
import PrizeList from 'app/fuse-layouts/prizes/PrizeList';
import NewPrize from 'app/fuse-layouts/prizes/NewPrize';
import Profile from '../../fuse-layouts/profile/MyProfile';
import NewPromotion from '../../fuse-layouts/promotions/NewPromotion';
import Promotions from '../../fuse-layouts/promotions/Promotions';
import UpdatePromotion from '../../fuse-layouts/promotions/UpdatePromotion';
import NewAuthorWithourAccount from 'app/fuse-layouts/authors/NewAuthorWithoutAccount';
import EditAuthorWithoutAccount from 'app/fuse-layouts/authors/EditAuthorWithoutAccount';
import Communities from 'app/fuse-layouts/communities/Communities';
import NewCommunity from 'app/fuse-layouts/communities/NewCommunity';
import UpdateCommunity from 'app/fuse-layouts/communities/UpdateCommunity';

const ExampleConfig = {
	settings: {
		layout: {
			config: {}
		}
	},
	routes: [
		{
			path: '/admin',
			component: Dashboard
		},
		{
			path: '/book/comments',
			component: Comments
		},
		{
			path: '/book/list/:id',
			component: UpdateBook
		},
		{
			path: '/book/list',
			component: BooksList
		},
		{
			path: '/book/new',
			component: NewBook
		},
		{
			path: '/subscriber/list/new',
			component: NewSubscriber
		},
		{
			path: '/subscriber/list/:id',
			component: EditSubscriber
		},
		{
			path: '/subscriber/list',
			component: Subscribers
		},
		{
			path: '/categories/packs',
			component: Packs
		},

		{
			path: '/cms/slides/new',
			component: NewSlide
		},
		{
			path: '/cms/slides',
			component: Slides
		},
		{
			path: '/cms/advertisement',
			component: Advertisement
		},
		{
			path: '/cms/offers',
			component: Offers
		},
		{
			path: '/cms/blogs',
			component: Blogs
		},

		{
			path: '/author/list/new',
			component: NewAuthor
		},

		{
			path: '/author/list/:id',
			component: EditAuthor
		},
		{
			path: '/author/list',
			component: Authors
		},
		{
			path: '/ask/editors/:id',
			component: NewEditor
		},
		{
			path: '/editor/list/new',
			component: NewEditor
		},
		{
			path: '/editor/list/:id',
			component: EditEditor
		},
		{
			path: '/editor/list',
			component: Editors
		},

		{
			path: '/categories',
			component: Categories
		},
		{
			path: '/ask/editors/:id',
			component: NewEditor
		},
		{
			path: '/ask/editors',
			component: EditorsJoin
		},

		{
			path: '/ask/authors/:id',
			component: NewAuthor
		},
		{
			path: '/ask/authors',
			component: AuthorsJoin
		},
		{
			path: '/authorWithoutAccount/list/new',
			component: NewAuthorWithourAccount
		},
		{
			path: '/authorWithoutAccount/list/:id',
			component: EditAuthorWithoutAccount
		},
		{
			path: '/authorWithoutAccount/list',
			component: AuthorsWithoutAccount
		},

		{
			path: '/bookPrize/list',
			component: PrizeList
		},
		{
			path: '/bookPrize/new',
			component: NewPrize
		},
		{
			path: '/profile',
			component: Profile
		},
		{
			path: '/bills/list',
			component: Bills
		},

		{
			path: '/promotion/new',
			component: NewPromotion
		},
		{
			path: '/promotion/list/:id',
			component: UpdatePromotion
		},
		{
			path: '/promotion/list',
			component: Promotions
		},

		{
			path: '/community/new',
			component: NewCommunity
		},
		{
			path: '/communities/list/:id',
			component: UpdateCommunity
		},
		{
			path: '/communities/list',
			component: Communities
		}
	]
};

export default ExampleConfig;

/**
 * Lazy load Example
 */
/*
import React from 'react';

const ExampleConfig = {
    settings: {
        layout: {
            config: {}
        }
    },
    routes  : [
        {
            path     : '/example',
            component: React.lazy(() => import('./Example'))
        }
    ]
};

export default ExampleConfig;

*/

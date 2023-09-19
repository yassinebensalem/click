import React from 'react';
import { Redirect } from 'react-router-dom';
import FuseUtils from '@fuse/utils/FuseUtils';
import DashboardConfig from 'app/main/dashboard/DashboardConfig';

const routeConfigs = [DashboardConfig];



const routes = [
	...FuseUtils.generateRoutesFromConfigs(routeConfigs),
	{
		path: '/',
		component: () => <Redirect to="/admin" />
	},

	{
		path: '/book/list',
		component: () => <Redirect to="/book/list" />
	},
	{
		path: '/book/list/:id',
		component: () => <Redirect to="/book/list/:id" />
	},

	{
		path: '/book/comments',
		component: () => <Redirect to="/book/comments" />
	},
	{
		path: '/book/new',
		component: () => <Redirect to="/book/new" />
	},
	{
		path: '/author/list',
		component: () => <Redirect to="/author/list" />
	},
	{
		path: '/authorWithoutAccount/list/new',
		component: () => <Redirect to="/authorWithoutAccount/list/new" />
	},
	{
		path: '/authorWithoutAccount/list/:id',
		component: () => <Redirect to="/authorWithoutAccount/list/:id" />
	},
	{
		path: '/authorWithoutAccount',
		component: () => <Redirect to="/authorWithoutAccount" />
	},

	{
		path: '/subscriber/list',
		component: () => <Redirect to="/subscriber/list" />
	},
	{
		path: '/subscriber/list/new',
		component: () => <Redirect to="/subscriber/list/new" />
	},
	{
		path: '/subscriber/list/:id',
		component: () => <Redirect to="/subscriber/list/:id" />
	},

	{
		path: '/ask/editors',
		component: () => <Redirect to="/ask/editors" />
	},
	{
		path: '/ask/authors/:id',
		component: () => <Redirect to="/ask/authors/:id" />
	},
	{
		path: '/ask/authors',
		component: () => <Redirect to="/ask/authors" />
	},

	{
		path: '/editor/list',
		component: () => <Redirect to="/editor/list" />
	},
	{
		path: '/editor/list/:id',
		component: () => <Redirect to="/editor/list/:id" />
	},

	{
		path: '/editor/list/new',
		component: () => <Redirect to="/editor/list/new" />
	},
	{
		path: '/ask/editors/:id',
		component: () => <Redirect to="/editor/new" />
	},
	{
		path: '/ask/editors',
		component: () => <Redirect to="/ask/editors" />
	},

	{
		path: '/categories',
		component: () => <Redirect to="/categories" />
	},
	{
		path: '/categories/new',
		component: () => <Redirect to="/categories/new" />
	},
	{
		path: '/categories/packs',
		component: () => <Redirect to="/categories/packs" />
	},
	{
		path: '/categories/orders',
		component: () => <Redirect to="/categories/orders" />
	},
	{
		path: '/cms/slides/new',
		component: () => <Redirect to="/cms/slides/new" />
	},
	{
		path: '/cms/slides',
		component: () => <Redirect to="/cms/slides" />
	},
	{
		path: '/cms/advertisement',
		component: () => <Redirect to="/cms/advertisement" />
	},
	{
		path: '/cms/blogs',
		component: () => <Redirect to="/cms/blogs" />
	},

	{
		path: '/bookPrize/list',
		component: () => <Redirect to="/bookPrize/list" />
	},
	{
		path: '/bookPrize/new',
		component: () => <Redirect to="/bookPrize/new" />
	},
	{
		path: '/profile',
		component: () => <Redirect to="/profile" />
	},
	{
		path: '/bills/list',
		component: () => <Redirect to="/bills/list" />
	},
	{
		path: '/promotion/new',
		component: () => <Redirect to="/promotion/new" />
	},
	{
		path: '/promotion/list/:id',
		component: () => <Redirect to="/promotion/list/:id" />
	},
	{
		path: '/promotion/list',
		component: () => <Redirect to="/promotion/list" />
	},
	{
		path: '/communities/list',
		component: () => <Redirect to="/communities/list" />
	},
	{
		path: '/community/new',
		component: () => <Redirect to="/community/new" />
	},
	{
		path: '/communities/list/:id',
		component: () => <Redirect to="/communities/list/:id" />
	},

];

export default routes;

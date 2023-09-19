// Internet Explorer 11 requires polyfills and partially supported by this project.
// import 'react-app-polyfill/ie11';
// import 'react-app-polyfill/stable';
import React from 'react';
import ReactDOM from 'react-dom';
import 'typeface-muli';
import './i18n';
import './styles/index.css';
import App from 'app/App';
import * as serviceWorker from './serviceWorker';
import reportWebVitals from './reportWebVitals';
import { BrowserRouter, Switch, Route, Redirect } from 'react-router-dom';
import Login from 'app/Login';
import { Provider } from 'react-redux';
import store from './app/store/index';
import ProtectedRoute from 'app/fuse-layouts/shared-components/ProtectedRoute';

import { persistStore } from 'redux-persist';

let persistor = persistStore(store);
ReactDOM.render(
	<Provider store={store}>
		<BrowserRouter>
			<Switch>
				<ProtectedRoute path="/admin" component={App} />
				<ProtectedRoute path="/author/list" component={App} />
				<ProtectedRoute path="/author/new" component={App} />
				<ProtectedRoute path="/editor/list" component={App} />
				<ProtectedRoute path="/editor/list/new" component={App} />
				<ProtectedRoute path="/ask/editors" component={App} />
				<ProtectedRoute path="/ask/authors" component={App} />
				<ProtectedRoute path="/subscriber/list" component={App} />
				<ProtectedRoute path="/subscriber/list/new" component={App} />
				<ProtectedRoute path="/subscriber/list/:id" component={App} />
				<ProtectedRoute path="/categories/packs" component={App} />
				<ProtectedRoute path="/categories" component={App} />
				<ProtectedRoute path="/book/list" component={App} />
				<ProtectedRoute path="/book/new" component={App} />
				<ProtectedRoute path="/book/comments" component={App} />
				<ProtectedRoute path="/orders" component={App} />
				<ProtectedRoute path="/cms/slides" component={App} />
				<ProtectedRoute path="/cms/advertisement" component={App} />
				<ProtectedRoute path="/cms/offers" component={App} />
				<ProtectedRoute path="/cms/blogs" component={App} />
				<ProtectedRoute path="/bills/list" component={App} />
				<ProtectedRoute path="/ask/authors" component={App} />
				<ProtectedRoute
					path="/authorWithoutAccount/list"
					component={App}
				/>
				<ProtectedRoute
					path="/authorWithoutAccount/list/new"
					component={App}
				/>
				<ProtectedRoute path="/editorWithoutAccount" component={App} />
				<ProtectedRoute path="/bookPrize/list" component={App} />
				<ProtectedRoute path="/bookPrize/new" component={App} />
				<ProtectedRoute path="/profile" component={App} />
				<ProtectedRoute path="/promotion/list/:id" component={App} />
				<ProtectedRoute path="/promotion/list" component={App} />
				<ProtectedRoute path="/promotion/new" component={App} />
				<ProtectedRoute path="/communities/list/:id" component={App} />
				<ProtectedRoute path="/communities/list" component={App} />
				<ProtectedRoute path="/community/new" component={App} />
				<Route
					exact
					path="/"
					render={() => {
						return localStorage.getItem('token') ? (
							<Redirect to="/admin" />
						) : (
								<Login />
							);
					}}
				/>
			</Switch>

		</BrowserRouter>

	</Provider>,
	document.getElementById('root')
);

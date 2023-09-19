import MomentUtils from '@date-io/moment';
import FuseAuthorization from '@fuse/core/FuseAuthorization';
import FuseLayout from '@fuse/core/FuseLayout';
import FuseTheme from '@fuse/core/FuseTheme';

import { createGenerateClassName, jssPreset, StylesProvider } from '@material-ui/core/styles';
import { MuiPickersUtilsProvider } from '@material-ui/pickers';
import { create } from 'jss';
import jssExtend from 'jss-plugin-extend';
import rtl from 'jss-rtl';
import React, { useEffect } from 'react';
import Provider from 'react-redux/es/components/Provider';
import { Router, useHistory, Redirect } from 'react-router-dom';
import AppContext from './AppContext';
import { Auth } from './auth';
import routes from './fuse-configs/routesConfig';
import Login from './Login';
import { useDispatch, useSelector } from 'react-redux';
import { getAuthMe, getCurrent, getRefresh } from './store/authSlice';
const jss = create({
	...jssPreset(),
	plugins: [...jssPreset().plugins, jssExtend(), rtl()],
	insertionPoint: document.getElementById('jss-insertion-point')
});

const generateClassName = createGenerateClassName();

const App = () => {
	const dispatch = useDispatch();
	const { isAuth, user, loading } = useSelector(state => state.auth);
	const history = useHistory();

	useEffect(() => {
		dispatch(getCurrent());
	}, []);

	if (user === '' && isAuth === false) {
		//history.push('/');
		return (<AppContext.Provider
			value={{
				routes
			}}
		>
			<StylesProvider jss={jss} generateClassName={generateClassName}>
				<MuiPickersUtilsProvider utils={MomentUtils}>
					<Auth>
						<Router history={history}>
							<FuseAuthorization>
								<FuseTheme>
									<FuseLayout />
								</FuseTheme>
							</FuseAuthorization>
						</Router>
					</Auth>
				</MuiPickersUtilsProvider>
			</StylesProvider>
		</AppContext.Provider>
		);
	}
	return (
		<AppContext.Provider
			value={{
				routes
			}}
		>
			<StylesProvider jss={jss} generateClassName={generateClassName}>
				<MuiPickersUtilsProvider utils={MomentUtils}>
					<Auth>
						<Router history={history}>
							<FuseAuthorization>
								<FuseTheme>
									<FuseLayout />
								</FuseTheme>
							</FuseAuthorization>
						</Router>
					</Auth>
				</MuiPickersUtilsProvider>
			</StylesProvider>
		</AppContext.Provider>
	);
};

export default App;

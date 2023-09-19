import { Redirect, Route, useHistory } from 'react-router';
import { useDispatch, useSelector } from 'react-redux';
import React, {useEffect, useState} from 'react';
import { getRefresh, logout } from 'app/store/authSlice';
import { decode } from 'jsonwebtoken';

const ProtectedRoute = ({ component: Component, ...rest }) => {
	const { token, refreshToken} = useSelector(state => state.auth);
	const dispatch = useDispatch();
	const [redirect, setRedirect] = useState(false);
	const history = useHistory();
	//const myToken = localStorage.getItem('token');

	useEffect(() => {
		dispatch(getRefresh({
          'AccessToken': localStorage.getItem('token'),
		  'RefreshToken': localStorage.getItem('refresh')
		}));
	}, []);
// if(redirect === false) {
// 	localStorage.removeItem('token')
// }


// const d = new Date(0);
// d.setUTCSeconds(decode(token).exp);
// console.log(d);

	return <Route {...rest} render={props => ( token ? <Component {...props} /> : <Redirect to="/" />)} />;
};

export default ProtectedRoute;

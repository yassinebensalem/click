import AppBar from '@material-ui/core/AppBar';
import Avatar from '@material-ui/core/Avatar';
import { makeStyles } from '@material-ui/core/styles';
import Typography from '@material-ui/core/Typography';
import clsx from 'clsx';
import React, { useEffect, useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { getCurrent, logout, getAuthMe } from 'app/store/authSlice';

const useStyles = makeStyles(theme => ({
	root: {
		'&.user': {
			'& .username, & .email': {
				transition: theme.transitions.create('opacity', {
					duration: theme.transitions.duration.shortest,
					easing: theme.transitions.easing.easeInOut
				})
			}
		}
	},
	avatar: {
		width: 72,
		height: 72,
		position: 'absolute',
		top: 92,
		padding: 8,
		background: '#80080',
		boxSizing: 'content-box',
		color: '#fff',
		left: '50%',
		transform: 'translateX(-50%)',
		transition: theme.transitions.create('all', {
			duration: theme.transitions.duration.shortest,
			easing: theme.transitions.easing.easeInOut
		})
	}
}));

function UserNavbarHeader(props) {
	const dispatch = useDispatch();
	const { user, loading, isAuth, firstName, lastName } = useSelector(
		state => state.auth
	);
	const classes = useStyles();
	const stringToColor = string => {
		let hash = 0;
		let i;

		/* eslint-disable no-bitwise */
		for (i = 0; i < string.length; i += 1) {
			hash = string.charCodeAt(i) + ((hash << 5) - hash);
		}

		let color = '#';

		for (i = 0; i < 3; i += 1) {
			const value = (hash >> (i * 8)) & 0xff;
			color += `00${value.toString(16)}`.substr(-2);
		}
		/* eslint-enable no-bitwise */

		return color;
	};
	const stringAvatar = name => {
		return {
			sx: {
				bgcolor: stringToColor(name)
			},
			children: `${name.split(' ')[0][0]}${name.split(' ')[1][0]}`
		};
	};
	useEffect(async () => {
		//dispatch(getCurrent());
		dispatch(getCurrent());
		//localStorage.removeItem('token')
		if (user === undefined) {
			localStorage.removeItem('token');
			if (localStorage.getItem('token') === undefined) {
				dispatch(logout());
			}
		}
	}, []);

	const convertString = str => {
		return str.split('@')[0].split('.').join(' ');
	};

	return (
		<AppBar
			position="static"
			color="primary"
			classes={{ root: classes.root }}
			className="user relative flex flex-col items-center justify-center pt-24 pb-64 mb-32 z-0 shadow-0"
		>
			<Typography
				className="username text-16 whitespace-nowrap"
				color="inherit"
			>
				<b>Espace Admin</b>
			</Typography>
			<Typography
				className="email text-13 mt-8 opacity-50 whitespace-nowrap"
				color="inherit"
			>
				{user}
			</Typography>
			{user && (
				<Avatar
					className={clsx(classes.avatar, 'avatar')}
					{...stringAvatar(`${firstName} ${lastName}`)}
				/>
			)}
		</AppBar>
	);
}

export default UserNavbarHeader;

import React, { useEffect } from 'react';
import picture from '../../../images/carl.jpg';
import styled from 'styled-components';
import Menu from '@material-ui/core/Menu';
import MenuItem from '@material-ui/core/MenuItem';
import Button from '@material-ui/core/Button';
import { makeStyles, Avatar } from '@material-ui/core';
import Person from '@material-ui/icons/PersonOutline';
import Exit from '@material-ui/icons/ExitToApp';
import { useHistory, Redirect, NavLink } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { getCurrent, logout } from 'app/store/authSlice';
import clsx from 'clsx';
const LINKCONTAINER = styled.a`
	display: flex;
	text-decoration: none;
	align-items: center;
	justify-content: space-between;
	min-width: 125px;
	margin-left: 10px;
`;
const IMAGE = styled.img`
	border-radius: 50%;
	width: 36px;
	height: 36px;
`;
const SPAN = styled.span`
	color: #000;
	font-size: 1.3rem;
	margin-left: 10px;
`;

const useStyles = makeStyles({
	icon: {
		marginRight: '10px',
		color: 'rgba(0, 0, 0, 0.5)'
	},
	btn: {
		justifyContent: 'space-between',
		width: '147px',
		height: '50px',
		paddingLeft: '1.6rem',
		paddingRight: '1.6rem',
		display: 'flex',
		alignItems: 'center'
	}
});
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

const Profile = () => {
	const [anchorEl, setAnchorEl] = React.useState(null);
	const history = useHistory();
	const dispatch = useDispatch();
	const { user, firstName, lastName } = useSelector(state => state.auth);
	function handleClick(event) {
		setAnchorEl(event.currentTarget);
	}

	const classes = useStyles();

	const handleClose = () => {
		setAnchorEl(null);
	};

	useEffect(() => {}, []);

	return (
		<div>
			<Button
				aria-owns={anchorEl ? 'simple-menu' : undefined}
				aria-haspopup="true"
				onClick={handleClick}
				style={{
					justifyContent: 'space-between',
					width: '165px',
					height: '50px',
					paddingLeft: '1.6rem',
					paddingRight: '1.6rem',
					display: 'flex',
					alignItems: 'center'
				}}
			>
				<div className="flex flex-col" style={{ paddingTop: '10px' }}>
					<span style={{ fontWeight: '400' }}>
						<strong>{`${firstName} ${lastName}`} </strong>
					</span>
					<span
						style={{
							fontSize: '11px',
							textAlign: 'right',
							display: 'block',
							marginBottom: '9px',
							color: 'rgb(107, 114, 128)'
						}}
					>
						Admin
					</span>
				</div>
				{user && (
					<Avatar className={clsx(classes.avatar, 'avatar')} {...stringAvatar(`${firstName} ${lastName}`)} />
				)}
			</Button>
			<Menu
				id="simple-menu"
				anchorEl={anchorEl}
				open={Boolean(anchorEl)}
				onClose={handleClose}
				PaperProps={{
					style: {
						marginTop: '40px',
						width: '180px'
					}
				}}
			>
				<MenuItem
					onClick={() => {
						dispatch(logout());
						//if (!localStorage.getItem('token')) return <Redirect to="/" />;
						localStorage.removeItem('token');
						history.push('/');
					}}
				>
					<Exit color="primary" className={classes.icon} />
					Se Déconnecter
				</MenuItem>
				<MenuItem
					onClick={() => {
						history.push('/profile');
					}}
				>
					<Person color="primary" className={classes.icon} />
					Mon Profil
				</MenuItem>
			</Menu>
		</div>
	);
};

export default Profile;

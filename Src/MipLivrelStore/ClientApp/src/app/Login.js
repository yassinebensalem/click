import {
	Box,
	Button,
	Checkbox,
	FormControlLabel,
	TextField,
	makeStyles,
	InputAdornment,
	IconButton,
	Input
} from '@material-ui/core';
import React, { useState, useEffect } from 'react';
import './Login.css';
import { withStyles } from '@material-ui/core/styles';
import { Link, useHistory } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { login, rememberMe } from './store/authSlice';

import './fuse-layouts/styles.css';
import { unwrapResult } from '@reduxjs/toolkit';
import { Visibility, VisibilityOff } from '@material-ui/icons';
import Loading from './fuse-layouts/shared-components/Loading';
import { useRef } from 'react';
const useStyles = makeStyles({
	input: {
		color: 'white'
	},
	label: {
		color: '#000000'
	}
});

const Login = () => {
	const [email, setEmail] = useState('');
	const [password, setPassword] = useState('');
	const history = useHistory();
	const classes = useStyles();
	const [emailError, setEmailError] = useState('');
	const [passwordError, setPasswordError] = useState('');
	const [helperTextEmail, setHelperTextEmail] = useState('');
	const [helperTextPassword, setHelperTextPassword] = useState('');
	const [errorMessage, setErrorMessage] = useState('');
	const [showPassword, setShowPassword] = useState(false);

    const checkBoxRef = useRef(null);
	const [user, setUser] = useState(null);

	const dispatch = useDispatch();
	const {checked} = useSelector(state => state.auth);
    useEffect(() => {
		if(checked) {
			setEmail(localStorage.getItem('email'));
			setPassword(localStorage.getItem('password'));
	
		}else {
			setEmail('');
			setPassword('');


		}
	}, [])


	const onChangeEmail = e => {
		setEmail(e.target.value);
		setEmailError(false);
		setErrorMessage('');
		setHelperTextEmail('');
	};
	const onChangePassword = e => {
		setPassword(e.target.value);
		setPasswordError(false);
		setErrorMessage('');
		setHelperTextPassword('');
	};
	const handleLogin = async e => {
		e.preventDefault();

		let regex = new RegExp('[a-z0-9]+@[a-z]+.[a-z]{2,3}');

		if (regex.test(email.trim()) === false) {
			setEmailError(true);
			setHelperTextEmail('email invalide');
		}
		if (password === '') {
			setPasswordError(true);
			setHelperTextPassword('champ vide');
		}
		if(checked) {
			localStorage.setItem('email', email);
			localStorage.setItem('password', password);
			dispatch(rememberMe(true))
		}else {
			dispatch(rememberMe(false))
		}

		dispatch(login({ Email: email, Password: password }))
			.then(unwrapResult)
			.then(originalPromiseResult => {
			  history.push('/admin');
			  localStorage.setItem('checked', checked);
              localStorage.setItem('email', email);
			  localStorage.setItem('password', password);
			  localStorage.setItem('checked', checked)
				
			})
			.catch(rejectedValueOrSerializedError => {
				if (Array.isArray(rejectedValueOrSerializedError.errors)) {
					setErrorMessage('mot de passe et/ou email erronée(s)');
				}
			});

		
	};

	const handleClickShowPassword = () => {
		setShowPassword(!showPassword);
	};
	const handleChangeChecked = (e) => {
		dispatch(rememberMe(e.target.checked));
	}



	return (
		<div className="w-full  color-white height-full">
			<div className="flex flex-col md:flex-row">
				<div
					className="flex flex-col justify-center   grow-0	p-16 ml-0 text-center md:p-128 md:items-start md:shrink-0 md:flex-1 md:text-left font-semibold left"
					style={{ background: '#fff', color: '#000', borderRight: "1px solid #eee" }}
				>
					<img src="assets/images/logos/logo_miplivrel.png" alt="logo" 
					style={{marginLeft:"14px"}}

					/>
					<p
						className="text-32 font-semibold  font-semibold sm:text-44"
						style={{
							width: '500px',
							marginLeft: '45px'
						}}
					>
						Bienvenu Au <br /> BackOffice
					</p>
					<p className="mt-32 font-medium font-500" style={{ marginLeft: '45px' }}>
						Est ullamco excepteur deserunt duis et eiusmod sint Lorem.
						<br /> Veniam qui minim eiusmod ullamco elit
					</p>
				</div>
				<div className="bg-white MuiPaper-root MuiPaper-elevation MuiPaper-elevation1 MuiCard-root w-1/2   mx-auto m-16 md:m-0 rounded-20 md:rounded-none h-screen  justify-center items-center right">
					<div className="flex flex-col justify-center light-dark h-full text-center shadow-md">
						<h6
							className="MuiTypography-h6 mb-24 font-semibold  sm:text-24 muiltr-1p8n6p5"
							style={{ fontSize: '28px' }}
						>
							Connectez à votre compte
						</h6>
						<span className="text-red">{errorMessage}</span>
						<Box component="form">
							<div className="flex flex-col items-center">
								<TextField
									id="outlined-basic"
									variant="outlined"
									className="w-3/4"
									name="Email"
									onChange={onChangeEmail}
									placeholder="Email"
									inputProps={{ className: classes.input, style: { fontSize: 20 } }}
									label="Email"
									InputLabelProps={{
										style: { color: '#121212' }
									}}
									value={email}
									helperText={helperTextEmail}
									error={emailError}
									required
								/>
								<TextField
									id="outlined-basic"
									type={showPassword ? 'text' : 'password'}
									variant="outlined"
									className="w-3/4 mt-10"
									name="Password"
									onChange={onChangePassword}
									placeholder="Mot De Passe"
									label="Mot De Passe"
									inputProps={{ className: classes.input, style: { fontSize: 20 } }}
									InputLabelProps={{
										style: { color: '#121212' }
									}}
									InputProps={{
										endAdornment: (
											<InputAdornment position="end">
												<IconButton
													aria-label="toggle password visibility"
													onClick={handleClickShowPassword}
												//onMouseDown={handleMouseDownPassword}
												>
													{showPassword ? <VisibilityOff /> : <Visibility />}
												</IconButton>
											</InputAdornment>
										)
									}}
									value={password}
									error={passwordError}
									helperText={helperTextPassword}
									required
								/>
									<div className='flex items-center mt-10 w-3/4'>
									<FormControlLabel control={<Checkbox  defaultChecked={false} 
									 sx={{ '& .MuiSvgIcon-root': { fontSize: 28 } }}
									checked={checked ? true: false}
									onChange={handleChangeChecked} />} label="Se souvenir de moi"
									style={{fontSize:"20px"}}
									 />

							</div>
							</div>
						
							<div className="flex justify-between m-auto  items-center w-3/4">
								<Link
									style={{
										color: '#88CF00',
										textDecoration: 'underline'
									}}
									className="ml-18 mt-10"
								>
									Mot De Passe oublie?
								</Link>
							</div>
							<Button
								contained
								className="w-3/4 cursor-pointer"
								style={{
									padding: '13px',
									background: '#434649',
									borderRadius: '20px',
									marginTop: '10px',
									color: '#fff',
									textTransform: 'capitalize',
									fontSize: '15px',
									height: '34.75px'
								}}
								onClick={handleLogin}
							>
								Se connecter
							</Button>
						</Box>
					</div>
				</div>
			</div>
		</div>
	);
};

export default Login;

import React from 'react';
import { createStyles, fade, Theme, makeStyles } from '@material-ui/core/styles';
import SearchIcon from '@material-ui/icons/Search';
import InputBase from '@material-ui/core/InputBase';

const useStyles = makeStyles(theme =>
	createStyles({
		search: {
			position: 'relative',
			borderRadius: '1.6rem',
			backgroundColor: fade(theme.palette.common.white, 0.15),
			'&:hover': {
				backgroundColor: fade(theme.palette.common.white, 0.25)
			},
			marginLeft: 0,
			marginRight: 10,
			width: '100%',
			boxShadow: '0px 0.5px 0.5px 0px rgba(0,0,0,0.75);',
			[theme.breakpoints.up('sm')]: {
				marginLeft: theme.spacing(1),
				width: 'auto'
			}
		},
		searchIcon: {
			padding: theme.spacing(0, 1),
			height: '100%',
			position: 'absolute',
			pointerEvents: 'none',
			display: 'flex',
			alignItems: 'center',
			justifyContent: 'center'
		},
		inputRoot: {
			color: 'inherit'
		},
		inputInput: {
			padding: theme.spacing(1, 1, 1, 0),
			//vertical padding + font size from searchIcon
			paddingLeft: `calc(1em + ${theme.spacing(4)}px)`,
			transition: theme.transitions.create('width'),

			width: '1200px',
			[theme.breakpoints.up('sm')]: {
				width: '30ch',
				'&:focus': {
					width: '30ch'
				}
			}
		}
	})
);

const Search = ({ onInput, placeholder }) => {
	const classes = useStyles();
	return (
		<div className={classes.search}>
			<div className={classes.searchIcon}>
				<SearchIcon />
			</div>
			<InputBase
				placeholder={placeholder}
				classes={{
					root: classes.inputRoot,
					input: classes.inputInput
				}}
				inputProps={{ 'aria-label': 'search' }}
				style={{
					width: '296px'
				}}
				onInput={onInput}
			/>
		</div>
	);
};

export default Search;

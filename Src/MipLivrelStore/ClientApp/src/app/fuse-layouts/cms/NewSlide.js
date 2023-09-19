import { TextField, Box, Button, Tab, Tabs, AppBar } from '@material-ui/core';
import React, { useState } from 'react';
import FusePageCarded from '@fuse/core/FusePageCarded';
import { useStyles } from '@material-ui/pickers/views/Calendar/SlideTransition';
import ArrowBack from '@material-ui/icons/ArrowBack';
import user from '../../../images/profile.jpg';
import a11yProps from '../a11props';
import TabPanel from '../shared-components/TabPanel';
import { Link } from 'react-router-dom';
import book from '../../../images/book.png';
const NewSlide = () => {
	const [value, setValue] = useState(0);
	const classes = useStyles();
	const handleChange = e => {
		setValue(e.target.value);
	};
	const [Title, setTitle] = useState('');
	const [SubTitle, setSubTitle] = useState('');
	const [Description, setDescription] = useState('');
	const [SlideLink, setSlideLinK] = useState('');
	const onChangeTitle = e => {
		setTitle(e.target.value);
	};
	const onChangeSubTitle = e => {
		setSubTitle(e.target.value);
	};
	const onChangeDescription = e => {
		setDescription(e.target.value);
	};
	const onChangeSlideLink = e => {
		setSlideLinK(e.target.value);
	};

	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center">
					<div className="pt-10 pb-10">
						<div className="flex flex-col max-w-full min-w-0">
							<Link
								to="/cms/slides"
								className="flex items-center sm:mb-8"
								style={{
									color: 'white',
									textDecoration: 'none'
								}}
							>
								<ArrowBack fontSize="5px" />
								<b className="hidden sm:flex mx-4 font-medium">Slides</b>
							</Link>
						</div>
						<div className="flex items-center max-w-full">
							<img src={book} alt="user" className="w-32 sm:w-48 rounded" width={40} height={40} />
							<div className="flex flex-col mx-8">
								<h2>Nouveau Slide</h2>
							</div>
						</div>
					</div>
					<div className="flex">
						<Button
							variant="primary"
							className="mx-2 h-28 rounded-2xl pt-20 pb-20"
							style={{
								backgroundColor: '#88CF00',
								color: '#fff'
							}}
						>
							Annuler
						</Button>
					</div>
				</div>
			}
			contentToolbar={
				<div className={classes.root}>
					<AppBar position="static" color="default">
						<Tabs
							value={value}
							onChange={handleChange}
							indicatorColor="primary"
							textColor="primary"
							variant="scrollable"
							scrollButtons="auto"
							aria-label="scrollable auto tabs example"
						>
							<Tab label="Basic info" {...a11yProps(0)} />
						</Tabs>
					</AppBar>
				</div>
			}
			content={
				<TabPanel value={value} index={0}>
					<Box component="form">
						<TextField
							id="outlined-basic"
							onChange={onChangeTitle}
							label="Titre"
							placeholder="Titre"
							variant="outlined"
							className="w-full mt-10"
							required
							value={Title}
						/>
						<TextField
							id="outlined-basic"
							onChange={onChangeSubTitle}
							label="Sous Titre"
							placeholder="Sous Titre"
							variant="outlined"
							className="w-full mt-10"
							required
							value={SubTitle}
						/>
						<TextField
							id="outlined-basic"
							variant="outlined"
							placeholder="Description"
							className="w-full mt-10 h-300px"
							multiline
							required
							rows={Infinity}
							rowsMax={10}
							label="Description"
							onChange={onChangeDescription}
						/>
						<p className="mt-10 text-gray">
							<label>Image de slide</label>
							<input id="outlined-basic" variant="outlined" className="w-full mt-10" type="file" />
						</p>
						<TextField
							id="outlined-basic"
							onChange={onChangeSlideLink}
							label="Lien"
							placeholder="lien"
							variant="outlined"
							className="w-full mt-10"
							required
							value={SlideLink}
						/>
						<Button className="save-btn" variant="contained" color="primary" size="small">
							Confirmer
						</Button>
					</Box>
				</TabPanel>
			}
		/>
	);
};

export default NewSlide;

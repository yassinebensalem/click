import { useEffect, Fragment, useState } from 'react';
import FusePageSimple from '@fuse/core/FusePageSimple';
import { makeStyles } from '@material-ui/core/styles';
import React from 'react';
import { useSelector, useDispatch } from 'react-redux';
import { getActiveSubscribers, getNonConfirmedSubscribers, getSubscribers } from '../../store/subscriberSlice';
import { getBestSellers, getTotalPublishedBooksCount, getTotalSoldBooksCount } from 'app/store/bookSlice';
import { getBestEditors, getEditorsCount } from '../../store/editorSlice';

import GradientPaper from 'app/fuse-layouts/shared-components/PaperCard';
import BookIcon from '../../../images/BookIcon.png';
import HouseIcon from '../../../images/House.png';
import BookVector from '../../../images/BookVector.png';
import HouseVector from '../../../images/HouseVector.png';
import PieVector from '../../../images/PieVector.png';
import UsersVector from '../../../images/UsersVector.png';




const useStyles = makeStyles(theme => ({
	table: {
		minWidth: 650
	},
	tableRow: {
		height: 30
	},
	tableCell: {
		padding: "0px 16px"
	},
	cardContainer: {
		display: 'flex',
		flexDirection: 'row',
		alignItems: 'flex-start',
		padding: '24px',
		gap: '24px',
	},
	sectionTitle: {
		fontFamily: 'Roboto',
		fontStyle: 'normal',
		fontWeight: 400,
		fontSize: '24px',
		lineHeight: '133.4%',
		padding: '24px 24px 0px 24px',
	}
}));

function ExamplePage(props) {
	const classes = useStyles(props);
	const dispatch = useDispatch();
	const { booksCount, soldBooksCount, bestSellers} = useSelector(state => state.book)
	const { editorsCount, bestEditors } = useSelector(state => state.editor);
	const { subscribers, numberOfActiveSubscribers, nonConfirmedSubscribers } = useSelector(state => state.subscriber);

	useEffect(() => {
		dispatch(getTotalPublishedBooksCount())
		dispatch(getSubscribers())
		dispatch(getActiveSubscribers())
		dispatch(getEditorsCount());
		dispatch(getBestEditors())
		dispatch(getTotalSoldBooksCount())
		dispatch(getBestSellers())
		dispatch(getNonConfirmedSubscribers())
	}, []);


	return (
		<FusePageSimple
			classes={{
				root: classes.layoutRoot
			}}
			content={
				<Fragment>
					<div className={classes.sectionTitle}>Livres</div>
					<div className={classes.cardContainer}>
						<GradientPaper
							header={{title:'Nombre des livres publiés'}}
							content={[{firstChildText:booksCount, secondChildText: "Livres"}]}
							vector={<img src={BookVector} alt="book" />}
							image={<img src={BookIcon} alt="book" />}
						/>
						<GradientPaper
							header={{title:'Nombre des livres vendus'}}
							content={[{firstChildText:soldBooksCount, secondChildText: "Livres"}]}
							vector={<img src={BookVector} alt="book" />}
							image={<img src={BookIcon} alt="book" />}
						/>
						<GradientPaper
							header={{title:`Nombre de maisons d’édition`}}
							content={[{firstChildText:editorsCount, secondChildText: "Maisons d’édition"}]}
							vector={<img src={HouseVector} alt="house" />}
							image={<img src={HouseIcon} alt="house" />}
						/>
					</div>
					<div className={classes.cardContainer}>
					<GradientPaper
							header={{title:`Best Sellers`, subtitle:`Par nombre de ventes`}}
							content={bestSellers}
							vector={<img src={HouseVector} alt="house" />}
							image={<img src={HouseIcon} alt="house" />}
							bgColor={'linear-gradient(135deg, #FFF 0%, #FEDFDE 100%)'}
							height={415}
							variant={'large'}
						/>
							<GradientPaper
							header={{title:`Best Publishers`, subtitle:`Par nombre de livres vendus`}}
							content={bestEditors}
							vector={<img src={HouseVector} alt="house" />}
							image={<img src={HouseIcon} alt="house" />}
							bgColor={'linear-gradient(135deg, #FFF 0%, #FEDFDE 100%)'}
							height={415}
							variant={'large'}
						/>
					</div>
					<div className={classes.sectionTitle}>Abonnés</div>
					<div className={classes.cardContainer}>
					<GradientPaper
							header={{title:'Statistiques'}}
							content={[{firstChildText:subscribers.length, secondChildText:"Abonnés", vector:<img src={UsersVector} /> }, {firstChildText:numberOfActiveSubscribers, secondChildText:"Inscriptions confirmées", vector:<img src={UsersVector} /> }, {firstChildText:nonConfirmedSubscribers, secondChildText:"Inscriptions en attente de confirmation", vector:<img src={PieVector} /> }]}
							variant="stats"
						/>
					</div>
					<div className={classes.sectionTitle}>Ventes</div>
					<div className={classes.cardContainer}>
					<GradientPaper
							header={{title:`Nombre total des ventes`, subtitle:`Par jour`}}
							content={bestSellers}
							bgColor={'#FFF'}
							height={446}
							variant={'large'}
						/>
					<GradientPaper
							header={{title:`Valeur total des ventes`, subtitle:`Par jour`}}
							content={bestSellers}
							bgColor={'#FFF'}
							height={446}
							variant={'large'}
						/>
						</div>
				</Fragment>
			}
		/>
	);
}

export default ExamplePage;

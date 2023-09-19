import FusePageCarded from '@fuse/core/FusePageCarded';
import React, { forwardRef, useEffect, useMemo, useState } from 'react';
import {
	Category, CheckBox,
} from '@material-ui/icons';
import MaterialTable from 'material-table';
import '../styles.css';
import CategoriesService from 'app/services/CategoriesService';

import swal from 'sweetalert';
import { useDispatch, useSelector } from 'react-redux';
import { getCategories, addCategory, updateCategory, removeCategory } from 'app/store/categorySlice';
import { decode } from 'jsonwebtoken';
import { useHistory } from 'react-router';
import ExportExcel from '../shared-components/ExcelExport';



const Categories = () => {
	const options = {
		headers: {
			Authorization: `Bearer ${localStorage.getItem('token')}`
		}
	};

	const { categories } = useSelector(state => state.category);
	const [errorMessage, setErrorMessage] = useState(null);

	const dispatch = useDispatch();
	const {token} = useSelector(state => state.auth);
	const {history} = useHistory();



	const loadCategories = () => {
		dispatch(getCategories());
	};
	useEffect(() => {
		loadCategories();
		if (decode(token).exp * 1000 < Date.now()) {
			history.push('/')
       }
	}, []);
    

	const originalRows = categories.filter(category=> category.parentId === null ).map((row) => ({
		...row,
		categoryName: JSON.parse(row['categoryName']),
	}
	));
	const getSubCategories = (id) => {
		const originalRows = categories.filter(category=> category.parentId === id ).map((row) => ({
			...row,
			categoryName: JSON.parse(row['categoryName']).fr,
		}
		))
		return originalRows.map((subCategory) => subCategory.categoryName).join(',');
	}

	//excel data for cetegories
	const ExcelExportData = originalRows.map(originalRow => ({
		"fr": originalRow.categoryName['fr'],
		"en": originalRow.categoryName['en'],
		"ar": originalRow.categoryName['ar'],
		status: originalRow.status === true ? 'active' : 'non active',
		id: originalRow.id,
		"sous catégories": getSubCategories(originalRow.id)
	}));

	const rows = originalRows.map((originalRow) => ({
		"id": originalRow.id,
		"fr": originalRow.categoryName['fr'],
		"en": originalRow.categoryName['en'],
		"ar": originalRow.categoryName['ar'],
		status: originalRow.status,
	}));


	const renderChildRow =(arr, parentId) => {
		return arr.filter(obj => obj.parentId === parentId).map((row) => ({
			...row,
			categoryName: JSON.parse(row['categoryName']),
		}
		))
		.map((originalRow) => ({
			...originalRow,
			"id": originalRow.id,
			"fr": originalRow.categoryName['fr'],
			"en": originalRow.categoryName['en'],
			"ar": originalRow.categoryName['ar'],
			status: originalRow.status === 'VRAI' ? 'active' : 'non active',
		}));
		
	}

       

	const tableIcons = {
		Check: forwardRef((props, ref) => (
			<span
				class="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-green text-20 muiltr-1cpc5a8"
				aria-hidden="true"
			>
				check_circle
			</span>
		)),
		ThirdStateCheck: forwardRef((props, ref) => (
			<span
				class="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-red text-20 muiltr-1cpc5a8"
				aria-hidden="true"
			>
				remove_circle
			</span>
		)),
		Clear: forwardRef((props, ref) => (
			<span
				class="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-red text-20 muiltr-1cpc5a8"
				aria-hidden="true"
			>
				clear_circle
			</span>
		)),
		Add: forwardRef((props,ref) => {
	
			return(
				<div style={{
						backgroundColor: '#f75454',
						width:'180px',
						padding:'12px',
						borderRadius:'20px',
						color:"#fff",
						fontSize:'13px',
						marginBottom:'-4px'
					}}>

						Ajouter une   catégorie
					</div>
			)})
	  
	};
	
	const tableSubCategoryIcons = {
		Check: forwardRef((props, ref) => (
			<span
				class="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-green text-20 muiltr-1cpc5a8"
				aria-hidden="true"
			>
				check_circle
			</span>
		)),
		ThirdStateCheck: forwardRef((props, ref) => (
			<span
				class="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-red text-20 muiltr-1cpc5a8"
				aria-hidden="true"
			>
				remove_circle
			</span>
		)),
		Clear: forwardRef((props, ref) => (
			<span
				class="material-icons notranslate MuiIcon-root MuiIcon-fontSizeMedium text-red text-20 muiltr-1cpc5a8"
				aria-hidden="true"
			>
				clear_circle
			</span>
		)),
		Add: forwardRef((props,ref) => {
			
		
		return	(
		
		 <div style={{
				backgroundColor: '#f75454',
				width:'180px',
				padding:'12px',
				borderRadius:'20px',
				color:"#fff",
				fontSize:'13px',
				marginBottom:'-4px'
			}}>

				 Ajouter une sous  catégorie
			</div>
			)})
	}



	const findCategoryName = categoryName => {
		const categoryFound = categories.filter(
			category => JSON.parse(category['categoryName']).fr === categoryName
		);
		if (categoryFound.length > 0) {
			if(categoryFound[0].parentId === null){
				swal({
					title: 'Catégorie existante!',
					icon: 'error',
					time: 5000
				});
			}else {
				swal({
					title: 'Sous Catégorie existante!',
					icon: 'error',
					time: 5000
				});
			}
			
			return true;
		} else {
			return false;
		}
	};

	return (
		<FusePageCarded
			header={
				<div className="flex flex-1 justify-between items-center w-full">
					<div className="pt-10 pb-10  " >
						<div className="flex items-center">
							<Category className="mr-4" />
							<h2>Catégories</h2>
						</div>	
					</div>
					<ExportExcel excelData={ExcelExportData} fileName={"Categories"}/>
				</div>
			}
			content={
				<div className="flex-col mt-8">
					<span className="text-red ml-8  px-10">{errorMessage}</span>
                
					<MaterialTable
						title=""
						icons={tableIcons}
						detailPanel={rowData => {
				        return(
                         <div style={{ padding: '10px 50px 10px 50px' }}>           
							<MaterialTable
							 icons={tableSubCategoryIcons}
							  editable={{
								onRowAdd: newData => {
									
									return new Promise(resolve => {
										
										if (
											findCategoryName(
												newData.fr
											) === false
										) {
	                                      
											dispatch(
												addCategory(
													{
													categoryName: JSON.stringify({
														fr: newData.fr,
														en: newData.en,
														ar: newData.ar
													}),
													Status: newData.status,
													parentId: rowData.id,
												}
												)	
											)
												.then(res => {
													swal({
														title: 'Sous Catégorie ajoutée!',
														icon: 'success',
														time: 5000
													});			
													if (
														!newData.hasOwnProperty(
															'status'
														)
													) {
														newData = {
															...newData,
															status: false
														};
													}

												
									
												
												}).then(() => {
													loadCategories()
												})
												.catch(err => {
													console.log(err, newData);
												});
											
										}
	                 
										resolve();
									}, 600);
									
			
								},
								onRowUpdate: (newData, oldData) =>
								new Promise(resolve => {
									dispatch(updateCategory({
										id: oldData.id,
										categoryName: JSON.stringify({
											fr: newData.fr,
											en: newData.en,
											ar: newData.ar
										}),
										Status: newData.status,
										ParentId: oldData.parentId,
										options
									}))
										.then(res => {
											swal({
												title: 'Catégorie modifiée!',
												icon: 'success',
												time: 5000
											});

										
										}).then(() => {
											loadCategories()
										})
										.catch(err => {
											console.log(err, newData);
										});
									resolve();
								}, 600),
							onRowDelete: oldData =>
								new Promise(resolve => {
									CategoriesService.remove(
										oldData.id,
										options
									).then(res => {
										swal({
											title: 'Catégorie supprimée!',
											icon: 'success',
											time: 5000
										});
										resolve();
										loadCategories();
									});
								})
							  }}
							  columns={
								[
									{
										title: 'Français',
										field: `fr`,
									},
									{
										title: 'Anglais',
										field: 'en',
									},
									{
										title: 'Arabe',
										field: 'ar',
									},
									{
										title: 'Active',
										field: 'status',
										type: 'boolean'
									}
								]
							  }
							 data={renderChildRow(categories, rowData.id)}
						
							
							  options={{
								paging: true,
								search: false,
								showTitle: false,
								actionsColumnIndex: 5,
						    	addRowPosition: 'first',
							    maxBodyHeight: 400
							  }}
							  
							/>
						  </div>
						)
							
						}}
						editable={{
							onRowAdd: newData => {
								setErrorMessage('');
		
								return new Promise(resolve => {
									if (
										findCategoryName(
											newData.fr
										) === false
									) {
										CategoriesService.create(
											{
												categoryName: JSON.stringify({
													fr: newData.fr,
													en: newData.en,
													ar: newData.ar
												}),
												Status: newData.status,
											},
										
										)
											.then(res => {
												swal({
													title: 'Catégorie ajoutée!',
													icon: 'success',
													time: 5000
												});
												if (
													!newData.hasOwnProperty(
														'status'
													)
												) {
													newData = {
														...newData,
														status: false
													};
												}
												loadCategories();
											})
											.catch(err => {
												console.log(err, newData);
											});
									}

									resolve();
								}, 600)


							},
							onRowUpdate: (newData, oldData) =>
								new Promise(resolve => {
									CategoriesService.update({
										id: oldData.id,
										categoryName: JSON.stringify({
											fr: newData.fr,
											en: newData.en,
											ar: newData.ar
										}),
										Status: newData.status,
										options
									})
										.then(res => {
											swal({
												title: 'Catégorie modifiée!',
												icon: 'success',
												time: 5000
											});

											loadCategories();
										})
										.catch(err => {
											console.log(err, newData);
										});
									resolve();
								}, 600),
							onRowDelete: oldData =>
								new Promise(resolve => {
									dispatch(removeCategory(
										oldData.id,
										options
									)).then(res => {
										swal({
											title: 'Catégorie supprimée!',
											icon: 'success',
											time: 5000
										});
										resolve();
										loadCategories();
									});
								})
						}}
						options={{
							actionsColumnIndex: 5,
							addRowPosition: 'first',
							search: false,
							maxBodyHeight: 400
						}}
						columns={[
							{
								title: 'Français',
								field: `fr`,
							},
							{
								title: 'Anglais',
								field: 'en',
							},
							{
								title: 'Arabe',
								field: 'ar',
							},
							{
								title: 'Active',
								field: 'status',
								type: 'boolean'
							}
						]}
						data={rows}
						// actions={[
						// 	{
						// 		    icon: 'add',
						// 			tooltip: 'Ajouter une sous catégorie',
						// 			onClick: (event, rowData) => {
						// 				// Do save operation
						// 				setExpandable(true);
						// 			}
						// 	}
						// ]}
					/>
				</div>
			}
		/>
	);
};

export default Categories;

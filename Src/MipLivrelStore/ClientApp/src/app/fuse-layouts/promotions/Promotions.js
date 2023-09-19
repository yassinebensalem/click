import React, { useState, useEffect, Fragment } from 'react'
import { DataGrid } from '@material-ui/data-grid';
import FusePageCarded from '@fuse/core/FusePageCarded';
import { Add, Person } from '@material-ui/icons';
import { Link } from 'react-router-dom';
import {
    Button, Toolbar,

} from '@material-ui/core';
import { useSelector, useDispatch } from 'react-redux';
import { getAllPromotions, deletePromotion } from 'app/store/promotionSlice';
import { useHistory } from 'react-router-dom';
import { unwrapResult } from '@reduxjs/toolkit';
import PromotionRow from '../shared-components/PromotionRow';
import swal from 'sweetalert';
import moment from 'moment';
import { makeStyles, createStyles } from '@material-ui/styles';
import { decode } from 'jsonwebtoken';
const useStyles = makeStyles(theme => ({

    paper: {

    }



}));
const Promotions = () => {
    const classes = useStyles();
    const tableStyle = {

        width: '100%',
        height: '100%'
    };
    const { promotions } = useSelector(state => state.promotion);
    const dispatch = useDispatch();
    const history = useHistory();

    const {token} = useSelector(state => state.auth);
    const getTypeName = (id) => {

        if (id === 0) {
            return "Gratuit"
        } else {
            return "Remise"
        }
    }

    const columns = [
        {
            field: 'name',
            headerName: 'Nom',
            flex: 1,
            headerAlign: "center",
            align: "center",

        },
        {
            field: 'type',
            headerName: 'Type',
            flex: 1,
            headerAlign: 'center',
            align: 'center'
        },
        {
            field: 'startDate',
            headerName: "Date De Début",
            flex: 1,
            headerAlign: 'center',
            align: 'center'
        },
        {
            field: 'endDate',
            headerName: "Date De Fin",
            flex: 1,
            headerAlign: 'center',
            align: 'center'
        },
        {
            field: 'percentage',
            headerName: "Pourcentage",
            flex: 1,
            headerAlign: 'center',
            renderCell: (params) => {
                if (params.row.percentage === null) {
                    return <div
                        style={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            width: '100%',
                        }}
                    >-</div>
                } else {
                    return <div
                        style={{
                            display: 'flex',
                            justifyContent: 'center',
                            alignItems: 'center',
                            width: '100%'
                        }}
                    >{params.row.percentage}</div>
                }
            }
        },
        {
            field: 'actions',
            headerName: 'Actions',
            width: 170,
            renderCell: params => {
                const onUpdateClick = e => {
                    history.push(`/promotion/list/${params.row.id}`);
                };
                const onDeleteClick = () => {
                    swal({
                        title: "Vous etes sure ??",
                        icon: "warning",
                        buttons: ["Non", "Oui"],
                        dangerMode: true,
                        confirmButtonText: "Oui",
                    })
                        .then((willDelete) => {
                            if (willDelete) {

                                dispatch(deletePromotion(params.row.id))
                                    .then(unwrapResult)
                                    .then(originalPromiseResult => {
                                        // handle result here

                                        swal({
                                            title: 'Promotion supprimée!',
                                            icon: 'success'
                                        });
                                        dispatch(getAllPromotions())

                                    })
                                    .catch(rejectedValueOrSerializedError => { });
                            }
                        });



                }
                return <PromotionRow update={onUpdateClick}
                    deletePromotion={onDeleteClick}
                />;
            }
        }
    ];
    useEffect(() => {
        dispatch(getAllPromotions());
        if (decode(token).exp * 1000 < Date.now()) {
			localStorage.clear();
			history.push('/')
        }

    }, []);
    const rows = promotions.map(promotion => ({
        ...promotion,
        startDate: moment(promotion.startDate).format(
            'YYYY-MM-DD'
        ),
        endDate: moment(promotion.endDate).format(
            'YYYY-MM-DD'
        ),
        type: getTypeName(promotion.promotionType),
    }));

    return (
        <FusePageCarded
            header={
                <div className="flex flex-1 justify-between items-center w-full">
                    <div className="pt-10 pb-10">
                        <div className="flex items-center">
                            <span class="material-icons MuiIcon-root-309 list-item-icon text-20 flex-shrink-0 MuiIcon-colorAction-312" aria-hidden="true"
                                style={{ fontSize: "30px" }}
                            >campaign</span>
                            <span className="ml-8 text-16 md:text-24 font-semiblod">
                                <b>Promotions</b>
                            </span>
                        </div>
                    </div>

                    <Link
                        to="/promotion/new"
                        style={{ textDecoration: 'none' }}
                    >
                        <Button
                            size="small"
                            variant="container"
                            className="save-btn"
                            color="primary"
                        >
                            <Add className="mr-8" />

                            Ajouter
                       </Button>
                    </Link>
                </div>
            }
            content={
                <div style={tableStyle}>
                    <DataGrid
                        className={classes.root}
                        toolBar={<Toolbar style={{ backgroundColor: "#f00" }} />}
                        rows={rows}
                        columns={columns}
                        pageSize={7}
                        rowsPerPageOptions={[10]}
                        disableColumnMenu
                        sx={{ backgroundColor: "#f00" }}
                        components={{
                            NoResultsOverlay: () => (
                                <div
                                    style={{
                                        display: 'flex',
                                        justifyContent: "center",
                                        alignItems: "center"
                                    }}
                                >
                                    pas de livres
                                </div>
                            )
                        }}

                        options={{
                            draggable: false
                        }}
                    />
                </div>
            }
        />
    )
}

export default Promotions

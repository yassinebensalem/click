import React, { useState, Fragment, useEffect } from 'react';
import {
    TextField,
    Box,
    Button
} from '@material-ui/core';
import communityIcon from '../../../images/community.png'
import FusePageCarded from '@fuse/core/FusePageCarded';
import { useDispatch } from 'react-redux';
import { ArrowBack } from '@material-ui/icons';
import { Link } from 'react-router-dom';
import { unwrapResult } from '@reduxjs/toolkit';
import swal from 'sweetalert';
import { createCommunity } from 'app/store/communitySlice';


const NewCommunity = () => {


    const dispatch = useDispatch();
    const [communityName, setCommunityName] = useState('');
    const [adminEmail, setAdminEmail] = useState('');


    const onChangeCommunityName = e => {
        setCommunityName(e.target.value);
    };    
    const onChangeAdminEmail = e => {
        setAdminEmail(e.target.value);
    };    


    const onSave = () => {
        const data = {
            "CommunityName": communityName,
            "adminEmail": adminEmail,
            "Status": true
        }    

        dispatch(createCommunity(data))
            .then(unwrapResult)
            .then(originalPromiseResult => {
                // handle result here
                swal({
                    title: 'Communeauté ajoutée!',
                    icon: 'success'
                });    
            })    
            .catch(rejectedValueOrSerializedError => { });
    };        

    return (
        <FusePageCarded
            header={
                <div className="flex flex-1 justify-between items-end pb-10">
                <div className="pt-10 pb-10">
                    <div className="flex flex-col max-w-full min-w-0">
                        <Link
                            to="/communities/list"
                            className="flex items-center sm:mb-8"
                            style={{
                                color: 'white',
                                textDecoration: 'none'
                            }}
                        >
                            <ArrowBack fontSize="5px" />
                            <b className="ml-8 text-16 md:text-24 font-semiblod">
                            Communautés
                            </b>
                        </Link>
                    </div>
                    <div className="flex items-center max-w-full">
                    <img src={communityIcon} />
                        <div className="flex flex-col mx-8">
                            <h2 className='MuiTypography-root MuiTypography-body1 text-16 sm:text-20 truncate font-semibold muiltr-ehddlr"'>
                                Ajouter une communauté
                            </h2>
                        </div>
                    </div>
                </div>
                
                        <Button
                            size="small"
                            variant="container"
                            className="save-btn"
                            color="primary"
                            onClick={onSave}
                            >
                            Sauvegarder
                       </Button>
            </div>
            }
            content={
                <div className='pt-48 px-32'>
                        <Box component="form">
                            <TextField
                                id="outlined-basic"
                                placeholder="Nom"
                                label={"Nom de la communauté" }
                                variant="outlined"
                                className="w-full mb-14"
                                name="name"
                                required
                                onChange={onChangeCommunityName}
                                value={communityName}
                            />
                            <TextField
                                id="outlined-basic"
                                placeholder="Email"
                                label="Email de l'admin"
                                variant="outlined"
                                className="w-full"
                                name="name"
                                required
                                onChange={onChangeAdminEmail
                                }
                                value={adminEmail}
                            />
                        </Box>
                </div>
            }
        />
    );
};

export default NewCommunity;

import React, { useState, Fragment, useEffect } from 'react';
import {
    TextField,
    Box,
    Button
} from '@material-ui/core';
import communityIcon from '../../../images/community.png'
import FusePageCarded from '@fuse/core/FusePageCarded';
import { useSelector, useDispatch } from 'react-redux';
import { ArrowBack } from '@material-ui/icons';
import { Link } from 'react-router-dom';
import { unwrapResult } from '@reduxjs/toolkit';
import swal from 'sweetalert';
import { getAllCommunities, getCommunityById, updateCommunity } from 'app/store/communitySlice';


const UpdateCommunity = ({ match }) => {

    useEffect(() => {
        dispatch(getCommunityById(match.params.id));
    }, []);

    const dispatch = useDispatch();
    const community = useSelector(state => state.communities.community)
    const [communityName, setCommunityName] = useState('');
    const [adminEmail, setAdminEmail] = useState('');


    useEffect(() => {
        if(community) {
            setCommunityName(community.communityName);
            setAdminEmail(community.admin ? community.admin.email : null);
        }
    }, [community])


    const onChangeCommunityName = e => {
        setCommunityName(e.target.value);
    };    
    const onChangeAdminEmail = e => {
        setAdminEmail(e.target.value);
    };    


    const onSave = () => {
        const data = {
            "id": match.params.id,
            "CommunityName": communityName,
            "AdminEmail": adminEmail,
            "Status": community.status
        }    

        dispatch(updateCommunity(data))
            .then(unwrapResult)
            .then(originalPromiseResult => {
                // handle result here
                dispatch(getAllCommunities());
                swal({
                    title: 'Communeauté modifiée!',
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
                                {communityName !== ''
                                    ? communityName
                                    : 'Modifier la communauté'}
                            </h2>
                            <span className="mt-4 text-xs">
                                Détails de la communauté
                            </span>
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

export default UpdateCommunity;

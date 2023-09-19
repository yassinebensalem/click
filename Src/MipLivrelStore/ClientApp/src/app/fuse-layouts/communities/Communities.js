import React, { StrictMode, useEffect, useState } from 'react'
import { DataGrid } from '@material-ui/data-grid';
import FusePageCarded from '@fuse/core/FusePageCarded';
import { Add } from '@material-ui/icons';
import { Link } from 'react-router-dom';
import {
    Button, Switch, Toolbar,

} from '@material-ui/core';
import { useSelector, useDispatch } from 'react-redux';
import { useHistory } from 'react-router-dom';
import PromotionRow from '../shared-components/PromotionRow';
import moment from 'moment';
import { deleteCommunity, getAllCommunities, updateCommunity } from 'app/store/communitySlice';

const StateCell = ({ community }) => {
    const [localValue, setLocalValue] = useState(community.state);
    const dispatch = useDispatch();
    
    const handleStateChange = () => {
        try {
            dispatch(updateCommunity({
                "id":community.id,
                "CommunityName":community.name,
                "AdminEmail":community.admin,
                "Status": Boolean(!localValue)   
            }));
            setLocalValue(!localValue);
        } catch {
            setLocalValue(!localValue)
        }
    };

    
    return (
      <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center', minWidth: 'inherit' }}>
    <Switch
        checked={localValue}
        onClick={handleStateChange}
        style={{
            '.MuiSwitchTrack':{
                backgroundColor:'white'
            }
        }}
      />
      <span style={{ marginLeft: 8 }}>{localValue.toString()}</span>
      </div>
    );
  };


const Communities = () => {

    const tableStyle = {
        width: '100%',
        height: '100%'
    };

    const { allCommunities } = useSelector(state => state.communities);
    const dispatch = useDispatch();
    const history = useHistory();

    const {token} = useSelector(state => state.auth);


    const columns = [
        {
            field: 'name',
            headerName: 'Nom',
            flex: 1,
            headerAlign: "center",
            align: "center",

        },
        {
            field: 'startDate',
            headerName: 'Date de création',
            flex: 1,
            headerAlign: 'center',
            align: 'center'
        },
        {
            field: 'admin',
            headerName: "Admin",
            flex: 1,
            headerAlign: 'center',
            align: 'center'
        },
        {
            field: 'state',
            headerName: "Etat",
            flex: 1,
            headerAlign: 'center',
            align: 'center',
            renderCell: params => <StateCell value={params.value} community={params.row} />
        },
        {
            field: 'actions',
            headerName: 'Actions',
            width: 170,
            renderCell: params => {
                const onUpdateClick = e => {
                    history.push(`/communities/list/${params.row.id}`);
                };
                const onDeleteClick = () => {
                    dispatch(deleteCommunity(params.row.id))
                }
                return <PromotionRow update={onUpdateClick}
                    deletePromotion={onDeleteClick}
                />;
            }
        }
    ];
    useEffect(() => {
        dispatch(getAllCommunities());
    }, []);


    const rows = allCommunities.map(community => ({
        id: community.id,
        name: community.communityName,
        startDate: moment(community.createdAt).format(
            'YYYY-MM-DD'
        ),
        admin: community.admin ? community.admin.email : null,
        state: community.status,
    }));

    return (
        <StrictMode>

        <FusePageCarded
            header={
                <div className="flex flex-1 justify-between items-center w-full">
                    <div className="pt-10 pb-10">
                        <div className="flex items-center">
                            <span class="material-icons MuiIcon-root-309 list-item-icon text-20 flex-shrink-0 MuiIcon-colorAction-312" aria-hidden="true"
                                style={{ fontSize: "30px" }}
                                >campaign</span>
                            <span className="ml-8 text-16 md:text-24 font-semiblod">
                                <b>Communautés</b>
                            </span>
                        </div>
                    </div>

                    <Link
                        to="/community/new"
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
            </StrictMode>
    )
}

export default Communities

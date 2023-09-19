import React, { StrictMode, useEffect, useState } from 'react'
import { DataGrid } from '@material-ui/data-grid';
import FusePageCarded from '@fuse/core/FusePageCarded';
import { InputBase, Link } from '@material-ui/core';
import { Toolbar } from '@material-ui/core';
import { makeStyles, createStyles } from '@material-ui/styles';

import communityIcon from '../../../images/community.png'
import { debounce } from 'lodash';
import { ArrowBack } from '@material-ui/icons';

const useStyles = makeStyles(() =>
createStyles({
    customToolbar: {
        width: '-webkit-fill-available',
        display: 'flex',
        justifyContent:'space-between',
        alignItems: 'center',
        height: 'inherit',
    },
    customToolbarCell: {
        padding:'16px 10px',
        height: 'inherit',
        lineHeight: '2em',
        fontWeight: 'bold'
    },
    customDataGridBorder: {
        borderTop: 'none !important'
    },
    centeredCell: {
        justifyContent: 'center',
    },
    inputRoot: {
        padding: 0
    },
})
);

const CommunityMembers = ({match}) => {
    const classes = useStyles(); 
    //const members = useSelector(state => state.community.communityMembers)
    const members = [
        { name: 'John Doe', startDate: '2023-01-01', email: 'john@example.com' },
        { name: 'Jane Smith', startDate: '2023-02-15', email: 'jane@example.com' },
        // Add more fake data as needed
      ];
    const [filteredMembers, setFilteredMembers] = useState(members);
    const [searchValue, setSearchValue] = useState('')
   // const dispatch = useDispatch();


    // useEffect(() => {
    //     dispatch(getCommunityMembers(match.params.id));
    // }, [match.params.id]);

    // useEffect(() => {
    //     members && setFilteredMembers(members);
    // }, [members]);
    
    const columns = [
            {
                field: 'member',
                headerName: 'Membres',
                flex: 1,
                headerAlign: "center",
                align: "center",
                cellClassName: classes.centeredCell,
            },
            {
                field: 'startDate',
                headerName: `Date d'ajout`,
                flex: 1,
                headerAlign: 'center',
                align: 'center',
                cellClassName: classes.centeredCell,
            },
            {
                field: 'email',
                headerName: "Email",
                flex: 1,
                headerAlign: 'center',
                align: 'center',
                cellClassName: classes.centeredCell,
            }
        ];


    const rows = filteredMembers.map(member => ({
            id: `${new Date().getTime().toString().concat(member.name.split(' ').join(''))}`,
            member: member.name,
            startDate: member.startDate,
            email: member.email
    }));
        
    const handleSearchInput = (value) =>  {
        let filteredTable = members.filter(member =>
            member.name.toLowerCase().includes(value.toLowerCase())
            )
        setFilteredMembers(filteredTable)
    }  
            
    const debouncedHandleSearchInput = (value) => debounce(() => handleSearchInput(value), 200);
            
    const onSearchInput = (event) => {
        setSearchValue(event.target.value);
        debouncedHandleSearchInput(event.target.value)()
    }

    return (
        <StrictMode>

        <FusePageCarded
            borderLess={true}
            header={
                <div className="flex flex-1 justify-between items-end w-full pb-10">
                    <div className="pt-10 pb-10">
                        <div className="flex items-center">
                            <Link
                            to="/communities/list"
                            className="flex items-center sm:mb-8"
                            style={{
                                color: 'white',
                                textDecoration: 'none'
                            }}
                        >
                            <span className='pl-2'><ArrowBack fontSize="5px" /></span>
                            <img src={communityIcon} />
                            <b className="ml-8 text-16 md:text-24 font-semiblod">
                            Communautés
                            </b>
                        </Link>
                        </div>
                        <div className='pt-12'>
                        <InputBase
				placeholder={"Rechercher"}
				inputProps={{ 'aria-label': 'search' }}
                classes={{
                    input: classes.inputRoot
                }}
				style={{
					padding: '15px',
                    borderRadius: '6px',
                    width: '274px',
                    backgroundColor: 'rgba(255, 255, 255, 0.15)'
				}}
                value={searchValue}
				onChange={onSearchInput}
			/>
                        </div>
                    </div>
                </div>
            }
            contentToolbar={
                <div className={classes.customToolbar}>
                    <div className={classes.customToolbarCell}>Communauté 1</div>
                    <div className={classes.customToolbarCell}>
                        <span>Admin </span>
                        <span>: Example@gmail.com</span>
                    </div>
                    <div className={classes.customToolbarCell}>Price</div>
                </div>
            }
            content={
                <div style={{
                    width: '100%',
                    height: '100%'
                }}>
                    <DataGrid
                        rows={rows}
                        columns={columns}
                        autoHeight={true}
                        disableColumnMenu
                        sx={{ backgroundColor: "#f00", 
                     }}
                    css={{
                        '& .MuiDataGrid-root': {
                            borderTop: 'none !important', // This will remove the top border from cells
                        },
                    }}
                        components={{
                            NoResultsOverlay: () => (
                                <div
                                style={{
                                    display: 'flex',
                                    justifyContent: "center",
                                    alignItems: "center"
                                }}
                                >
                                    pas de membres
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

export default CommunityMembers

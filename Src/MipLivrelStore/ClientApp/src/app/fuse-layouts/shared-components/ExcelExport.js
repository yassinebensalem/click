import React from 'react';
import { Button, Tooltip } from '@material-ui/core';
import * as FileSaver from 'file-saver';

import XLSX from 'sheetjs-style';


const fileType = 'application/vnd.openxmlformats-officedocument.spreadsheethtml.sheet;charset=UTF-8';
const fileExtenstion = '.xlsx';
const ExportExcel = ({excelData, fileName}) => {
   
  const exportToExcel = async () => {
    const ws = XLSX.utils.json_to_sheet(excelData);
    const wb = {Sheets: {'data': ws}, SheetNames: ['data']};
    const excelBuffer = XLSX.write(wb, {bookType:'xlsx', type:'array'});
    const data = new Blob([excelBuffer], {type: fileType});
    FileSaver.saveAs(data, fileName + fileExtenstion);
  }  


  return (
    <>
        <Tooltip title="Excel Export">
            <Button variant='contained'
            onClick={(e) => exportToExcel(fileName)}
            color="#F75454"
            style={{cursor:'pointer', fontSize:14, marginLeft:"5px", backgroundColor:"green", color:"#fff"}}
            >
                 Exporter Excel
            </Button>
        </Tooltip>
    </>
  )
  

}

export default ExportExcel;
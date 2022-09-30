using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GH.XlShablon
{
    internal class FieldsMap : List<FieldParam>
    {
        public static FieldsMap Instance = null;
        public FieldsMap()
        {
            //SET @distrib_code = < VARCHAR(20) >;
            Add(new FieldParam("Distrib's code", "distrib_code", typeof(string), 20, true));
            //SET @label_code = < VARCHAR(20) >;
            Add(new LabelCodeParam("Label's code", "label_code"));
            //SET @label_name = < VARCHAR(255) >;
            Add(new FieldParam("Label", "label_name", typeof(string), 20));
            //SET @d_price = < DOUBLE >;
            Add(new FieldParam("Price", "d_price", typeof(double), 0, true));
            //SET @barcode = < VARCHAR(13) >;
            Add(new BarcodeParam("Barcode EAN13", "barcode"));
            //SET @artist_name = < VARCHAR(255) >;
            Add(new FieldParam("Artist", "artist_name", typeof(string), 255, true));
            //SET @title = < VARCHAR(255) >;
            Add(new FieldParam("Title", "title", typeof(string), 255, true));
            //SET @units = < INT(6) >;
            Add(new FieldParam("Units at format", "units", typeof(int), 6));
            //SET @media_name = < VARCHAR(255) >;
            Add(new FieldParam("Format", "media_name", typeof(string), 255, true));
            //SET @genre_name = < VARCHAR(255) >;
            Add(new FieldParam("Genre", "genre_name", typeof(string), 255));
            //SET @release_date = < DATE >;
            Add(new ReleaseDateParam("Release date", "release_date"));
            //SET @note = < VARCHAR(255) >;
            Add(new FieldParam("Note", "note", typeof(string), 255));
            //SET @origin = < VARCHAR(12) >;
            Add(new FieldParam("Origin", "origin", typeof(string), 12));
            //SET @status_code = < CHAR(1) >;
            Add(new FieldParam("Status code", "status_code", typeof(string), 1));
            Instance = this;
        }


        new public void Clear()
        {
            foreach (var item in ToArray())
                item.Clear();
        }

        internal bool MapIsReady(bool showDlg)
        {
            foreach (var item in this)
            {
                if (!item.Validate(showDlg))
                    return false;
            };

            return true;
        }
    }


}

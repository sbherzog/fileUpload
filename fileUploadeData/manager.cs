using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileUploadeData
{
    public class managerDB
    {
        private string _connectonString;
        public managerDB(string connectionsString)
        {
            _connectonString = connectionsString;
        }

        public IEnumerable<imageClass> GetAllimages()
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM imageTable";
            connection.Open();
            SqlDataReader reder = command.ExecuteReader();
            List<imageClass> result = new List<imageClass>();
            while (reder.Read())
            {
                imageClass newImage = new imageClass();
                newImage.id = (int)reder["id"];
                newImage.title = (string)reder["title"];
                newImage.imageName = (string)reder["imageName"];
                newImage.dateListed = (DateTime)reder["dateListed"];
                result.Add(newImage);
            }
            return result;
        }

        public imageClass GetSingleListing(int id)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT * FROM imageTable WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            SqlDataReader reder = command.ExecuteReader();
            reder.Read();
            imageClass newImage = new imageClass();
            newImage.id = (int)reder["id"];
            newImage.title = (string)reder["title"];
            newImage.description = (string)reder["description"];
            if (reder["personName"] != DBNull.Value)
            {
                newImage.personName = (string)reder["personName"];
            }
            if (reder["cookieCode"] != DBNull.Value)
            {
                newImage.cookieCode = (string)reder["cookieCode"];
            }
            newImage.phoneNumber = (string)reder["phoneNumber"];
            newImage.imageName = (string)reder["imageName"];
            newImage.dateListed = (DateTime)reder["dateListed"];
            return newImage;
        }

        public void addNewListing(imageClass image, string fileName)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"INSERT INTO imageTable (title, description, personName, phoneNumber, imageName, dateListed, cookieCode) 
                        VALUES (@title, @description, @personName, @phoneNumber, @imageName, @dateListed, @cookieCode)";
            command.Parameters.AddWithValue("@title", image.title);
            command.Parameters.AddWithValue("@description", image.description);
            command.Parameters.AddWithValue("@personName", image.personName);
            command.Parameters.AddWithValue("@phoneNumber", image.phoneNumber);
            command.Parameters.AddWithValue("@imageName", fileName);
            command.Parameters.AddWithValue("@dateListed", image.dateListed);
            command.Parameters.AddWithValue("@cookieCode", image.cookieCode);
            connection.Open();
            command.ExecuteScalar();
        }

        public void DeleteListing(int id)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"DELETE FROM imageTable WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            command.ExecuteScalar();
        }

        public string fileName(int id)
        {
            SqlConnection connection = new SqlConnection(_connectonString);
            SqlCommand command = connection.CreateCommand();
            command.CommandText = @"SELECT imageName FROM imageTable WHERE id = @id";
            command.Parameters.AddWithValue("@id", id);
            connection.Open();
            string fileName = "";
            object value = command.ExecuteScalar();
            if (value != DBNull.Value)
            {
                fileName = (string)value;
            }
            return fileName;
        }

    }
}

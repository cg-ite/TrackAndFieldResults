/*
 * SPDX - FileCopyrightText: Copyright © 2022 Christian Günther <cg-ite@gmx.de>
 * SPDX - License - Identifier: GPL - 3.0 - or - later
 */

using System.Text;

namespace TrackAndFieldResults.Extentions
{
    public static class HttpContentExtensions
    {
        /// <summary>
        /// Speichert eine Http-Resource als File ab
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filename"></param>
        /// <param name="overwrite"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        /// <remarks>From https://stackoverflow.com/a/52395228</remarks>
        public static async Task DownloadFileTaskAsync(this HttpClient client, Uri uri, string FileName)
        {
            var j = await client.GetStringAsync(uri);

            using (var fs = new StreamWriter(FileName,
                    false, Encoding.UTF8))
            {
                fs.Write(j);
            }
        }
    }
}

const attachSorter = function() {
  const tables = document.querySelectorAll("#entity-table");
  for (let table of tables) {
    const headers = table.querySelectorAll("th");
    const tableBody = table.querySelector("tbody");
    const rows = tableBody.querySelectorAll("tr");

    // Track sort directions
    const directions = Array.from(headers).map(function(header) {
      return "";
    });

    // Transform the content of given cell in given column
    const transform = function(index, content) {
      // Get the data type of column
      const type = headers[index].getAttribute("data-type");

      if (type === "name") {
        content = content.childNodes[0].innerHTML;
      } else if (type === "number") {
        content = content.childNodes[0].childNodes[2].innerHTML;
      } else if (type === "date-range") {
        content = content.childNodes[0].innerHTML.split("-")[0];
      }

      switch (type) {
        case "number":
          return parseFloat(content);
        case "date":
          return Date.parse(content);
        case "string":
        default:
          return content;
      }
    };

    const sortColumn = function(index) {
      // Get the current direction
      const direction = directions[index] || "asc";

      // A factor based on the direction
      const multiplier = direction === "asc" ? 1 : -1;

      const newRows = Array.from(rows);

      newRows.sort(function(rowA, rowB) {
        const cellA = rowA.querySelectorAll("td")[index];
        const cellB = rowB.querySelectorAll("td")[index];

        const a = transform(index, cellA);
        const b = transform(index, cellB);

        switch (true) {
          case a > b:
            return 1 * multiplier;
          case a < b:
            return -1 * multiplier;
          case a === b:
            return 0;
        }
      });

      // Remove old rows
      [].forEach.call(rows, function(row) {
        tableBody.removeChild(row);
      });

      // Reverse the direction
      directions[index] = direction === "asc" ? "desc" : "asc";

      // Append new row
      newRows.forEach(function(newRow) {
        tableBody.appendChild(newRow);
      });
    };

    [].forEach.call(headers, function(header, index) {
      header.addEventListener("click", function() {
        sortColumn(index);
      });
    });
  }
};

const attachSearchClear = function() {
  // Hook into the rendered button
  const clearButton = document.getElementById("search-clear-button");
  if (clearButton) {
    clearButton.addEventListener("click", function() {
      document.getElementById("search-input").value = "";
      document.getElementById("search-form").submit();
    });
  }
};

window.addEventListener("DOMContentLoaded", function() {
  attachSorter();
  attachSearchClear();
});

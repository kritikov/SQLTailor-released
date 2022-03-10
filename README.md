SQL Tailor is an open source parser that formats SQL queries. It is built in WPF and C# and convert queries in various versions of SQL or in scripts that produce SQL, such as the SqlOM library.

It uses the Microsoft ScriptDOM library to parse the original query and then provides the translators who undertake to compose the resulting tokens in the desired versions.

It currently provides three translations:

Microsoft T-SQL: comes directly from the ScriptDOM library and formats the query given in T-SQL.

Base Translation: the basic translation of Tailor. Converts the most common queries and is used as a basis for other translators.

SqlOM Translation: builds the script of the SqlOM library in C #, which if executed generates the given SQL query.

A developer who wants a new variant of SQL can build a new translator without having to start all over. He may also use only the SQLParser that does the parsing instead of the entire application.

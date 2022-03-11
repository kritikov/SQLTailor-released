SQL Tailor is an open source parser that formats SQL queries. It is built in WPF and C# and convert queries in various versions of SQL or in scripts that produce SQL, such as the SqlOM library.

It uses the Microsoft ScriptDOM library to parse the original query and then provides the translators who compose the resulting tokens in the desired versions.

It currently provides three translations:

Microsoft T-SQL: comes directly from the ScriptDOM library and formats the query in T-SQL.

Base Translation: the basic translation of Tailor. Converts the most common queries and is used as a basis for other translators.

SqlOM Translation: builds the script of the SqlOM library in C #, which if executed generates the given SQL query.

A developer who wants a new variant of SQL can build a new translator without having to start all over. He may also use only the SQLParser that does the parsing instead of the entire application.


*** FOR DEVELOPERS ***

The project SQLParser is responsible for the communication with ScriptDOM and for the translators that composes the tokens. The steps for the translate procedure are:

- The ScriptDOM analyze the given query and creates a tree with the tokens. For every token, the ScriptDOM uses a specific class, as BinaryExpression class for example. There are over 1000 classes like this.
- The SQLParser.Parser gets the top tokens of this tree (might be more than one tokens, as two SELECT statements for example) and creates a list with them. Then send them to a translator.
- The translator has parsing functions for the token classes. For example, if the token is a SelectStatement class then the corresponding function is the SelectStatementParse. The traslator get the token and parses it with this function.
- Every parsing function searches the elements of the token (usually is a tree wich contains other tokens) and for each of them calls their corresponing parsing functions based on their class. That means that the parsing functions are recursive.
- The final result is a text but the tranlator can convert it to a FlowDocument with formatted words using its GetFlowDocument() method.

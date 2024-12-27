# OctoCrypto API

## Overview

The **OctoCrypto API** is an ASP.NET Core Web API designed to provide comprehensive information about cryptocurrency coins on various exchanges. Its purpose is to help users choose the best exchange for buying or selling coins by aggregating data from multiple platforms and presenting it in an easily accessible format.

---

## Features

- Collects data from the following exchanges:
  - **MEXC**
  - **Gate.io**
  - **BingX**
- Aggregates and caches the data for optimal performance.
- Provides a RESTful API to retrieve coin summaries.

---

## Architecture

### **Components**

1. **SymbolSummaryJob**  
   - Periodically fetches data from supported exchanges.  
   - Aggregates and processes the data.  
   - Saves results to a shared cache through `ISymbolSummaryCache`.

2. **ISymbolSummaryCache**  
   - A caching layer to store aggregated data for quick retrieval.  
   - Reduces the need for repeated API calls to exchanges.

3. **SymbolSummaryService**  
   - Retrieves cached data for API responses.  
   - Handles any additional business logic required for the data.

4. **SymbolSummaryController**  
   - Exposes endpoints to clients to retrieve symbol summaries.  
   - Utilizes `SymbolSummaryService` to fetch and return data.

---

## Unit Tests

### **SymbolSummaryControllerTests**

1. **First_summaries_page_retrieved_when_summary_job_has_worked_out**  
2. **First_summaries_page_retrieved_when_summary_job_has_worked_out_with_external_api_errors**  

### **SymbolSummaryJobTests**

1. **Symbol_summaries_are_saved_in_cache**

### **SymbolSummaryServiceTests**

1. **Specified_page_is_retrieved**
2. **First_page_is_retrieved_when_page_index_is_not_provided**
3. **Ten_summaries_are_retrieved_when_page_index_is_not_provided**
